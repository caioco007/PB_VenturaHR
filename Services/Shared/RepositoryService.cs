using DTO.Utils;
using Microsoft.EntityFrameworkCore;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Services.Shared
{
    public abstract class RepositoryService<TEntity, TModel, Key> : IRepositoryService<TEntity, TModel, Key>
        where TEntity : class
        where TModel : class
    {
        public DbSet<TEntity> dbSet;
        internal readonly ApplicationDbContext.Context.ApplicationDbContext context;
        internal readonly string keyName;
        internal readonly string table;
        internal readonly string schema;
        public RepositoryService(ApplicationDbContext.Context.ApplicationDbContext context, string keyName)
        {
            this.context = context;
            this.dbSet = (DbSet<TEntity>)context.GetType().GetProperty(typeof(TEntity).Name).GetValue(context);
            this.keyName = keyName;
            table = this.context.Model.FindEntityType(typeof(TEntity)).GetTableName();
            schema = this.context.Model.FindEntityType(typeof(TEntity)).GetSchema() ?? "dbo";
        }

        public TEntity GetDataById(Key id)
        {
            string query = $"SELECT * FROM [{schema}].[{table}] WHERE [{keyName}] = ";
            if (typeof(Key) == typeof(string)) query += $"'{id}'";
            else query += id;

            return dbSet.FromSqlRaw(query).SingleOrDefault();
        }
        public async Task<TEntity> GetDataByIdAsync(Key id)
        {
            string query = $"SELECT * FROM [{schema}].[{table}] WHERE [{keyName}] = ";
            if (typeof(Key) == typeof(string)) query += $"'{id}'";
            else query += id;

            return await dbSet.FromSqlRaw(query).SingleOrDefaultAsync();
        }
        public IQueryable<TEntity> GetData()
        {
            return (from l in this.dbSet select l);
        }
        public List<TEntity> GetData(Expression<Func<TEntity, bool>> expr = null)
        {
            if (expr != null)
                return this.dbSet.Where(expr).ToList();

            return dbSet.ToList();
        }
        public async Task<List<TEntity>> GetDataAsync(Expression<Func<TEntity, bool>> expr = null)
        {
            if (expr != null)
                return await dbSet.Where(expr).ToListAsync();

            return await dbSet.ToListAsync();
        }
        public List<TEntity> GetDataAsNoTracking(Expression<Func<TEntity, bool>> expr = null)
        {
            if (expr != null)
                return this.dbSet.Where(expr).AsNoTracking().ToList();

            return this.dbSet.AsNoTracking().ToList();
        }
        public async Task<List<TEntity>> GetDataAsNoTrackingAsync(Expression<Func<TEntity, bool>> expr = null)
        {
            if (expr != null)
                return await dbSet.Where(expr).AsNoTracking().ToListAsync();

            return await dbSet.AsNoTracking().ToListAsync();
        }

        public virtual TModel ToViewModel(TEntity data)
        {
            return data.CopyToEntity<TModel>();
        }
        public virtual List<TModel> ToViewModel(IEnumerable<TEntity> datas)
        {
            return datas.Select(x => x.CopyToEntity<TModel>()).ToList();
        }
        public TModel GetViewModelById(Key id)
        {
            return ToViewModel(GetDataById(id));
        }
        public List<TModel> GetViewModel(Expression<Func<TEntity, bool>> expr = null)
        {
            List<TEntity> entities;

            if (expr != null) entities = GetData(expr);
            else entities = GetData().ToList();

            return ToViewModel(entities);
        }
        public async Task<TModel> GetViewModelByIdAsync(Key id)
        {
            return ToViewModel(await GetDataByIdAsync(id));
        }
        public async Task<List<TModel>> GetViewModelAsync(Expression<Func<TEntity, bool>> expr = null)
        {
            List<TEntity> entities;

            if (expr != null) entities = (await GetDataAsync(expr));
            else entities = (await GetDataAsync());

            return ToViewModel(entities);
        }
        public List<TModel> GetViewModelAsNoTracking(Expression<Func<TEntity, bool>> expr = null)
        {
            List<TEntity> entities;

            if (expr != null) entities = GetDataAsNoTracking(expr);
            else entities = GetDataAsNoTracking();

            return ToViewModel(entities);
        }
        public async Task<List<TModel>> GetViewModelAsNoTrackingAsync(Expression<Func<TEntity, bool>> expr = null)
        {
            List<TEntity> entities;

            if (expr != null)
                entities = await GetDataAsNoTrackingAsync(expr);
            else entities = await GetDataAsNoTrackingAsync();

            return ToViewModel(entities);
        }
        public async Task<bool> Exist(Expression<Func<TEntity, bool>> expr) => await this.dbSet.AnyAsync(expr);

        public Key CreateOrUpdate(TModel model)
        {
            if (typeof(TModel).GetProperty(keyName).GetValue(model, null) == null) return Create(model);
            else Update(model);

            return (Key)typeof(TModel).GetProperty(keyName).GetValue(model);
        }
        public virtual async Task<Key> CreateOrUpdateAsync(TModel model)
        {
            if (typeof(TModel).GetProperty(keyName).GetValue(model, null) == null) return await CreateAsync(model);
            else await UpdateAsync(model);

            return (Key)typeof(TModel).GetProperty(keyName).GetValue(model);
        }

        public virtual Key Create(TModel model)
        {
            var entity = model.CopyToEntity<TEntity>();

            this.dbSet.Add(entity);
            this.context.SaveChanges();

            return (Key)typeof(TEntity).GetProperty(keyName).GetValue(entity);
        }
        public virtual async Task<Key> CreateAsync(TModel model)
        {
            var entity = model.CopyToEntity<TEntity>();

            await this.dbSet.AddAsync(entity);
            await this.context.SaveChangesAsync();

            return (Key)typeof(TEntity).GetProperty(keyName).GetValue(entity);
        }

        public virtual void Update(TModel model)
        {
            var id = (Key)typeof(TModel).GetProperty(keyName).GetValue(model);

            var entity = dbSet.Find(id);
            model.CopyToEntity<TEntity>(ref entity);

            this.dbSet.Update(entity);
            this.context.SaveChanges();
        }
        public virtual async Task UpdateAsync(TModel model)
        {
            var id = (Key)typeof(TModel).GetProperty(keyName).GetValue(model);

            var entity = await dbSet.FindAsync(id);
            model.CopyToEntity<TEntity>(ref entity);

            this.dbSet.Update(entity);
            await this.context.SaveChangesAsync();
        }

        public virtual void Update(TEntity data)
        {
            var id = (Key)typeof(TEntity).GetProperty(keyName).GetValue(data);

            var entity = dbSet.Find(id);
            data.CopyToEntity<TEntity>(ref entity, false);

            this.dbSet.Update(entity);
            this.context.SaveChanges();
        }
        public virtual async Task UpdateAsync(TEntity data)
        {
            var id = (Key)typeof(TEntity).GetProperty(keyName).GetValue(data);

            var entity = await dbSet.FindAsync(id);
            data.CopyToEntity<TEntity>(ref entity, false);

            this.dbSet.Update(entity);
            await this.context.SaveChangesAsync();
        }

        public virtual void Delete(Key id, int? userId = null)
        {
            var data = this.dbSet.Find(id);

            var isDel = typeof(TEntity).GetProperties().SingleOrDefault(x => x.Name.ToUpper() == "ISDELETED");
            if (isDel != null) isDel.SetValue(data, true);

            var delBy = typeof(TEntity).GetProperties().SingleOrDefault(x => x.Name.ToUpper() == "DELETEDBY");
            if (delBy != null && userId.HasValue) delBy.SetValue(data, userId);

            var delDate = typeof(TEntity).GetProperties().SingleOrDefault(x => x.Name.ToUpper() == "DELETEDDATE");
            if (delDate != null) delDate.SetValue(data, DateTime.Now);

            this.dbSet.Update(data);
            this.context.SaveChanges();
        }
        public virtual async Task DeleteAsync(Key id, int? userId = null)
        {
            var data = await this.dbSet.FindAsync(id);

            var isDel = typeof(TEntity).GetProperties().SingleOrDefault(x => x.Name.ToUpper() == "ISDELETED");
            if (isDel != null) isDel.SetValue(data, true);

            var delBy = typeof(TEntity).GetProperties().SingleOrDefault(x => x.Name.ToUpper() == "DELETEDBY");
            if (delBy != null && userId.HasValue) delBy.SetValue(data, userId);

            var delDate = typeof(TEntity).GetProperties().SingleOrDefault(x => x.Name.ToUpper() == "DELETEDDATE");
            if (delDate != null) delDate.SetValue(data, DateTime.Now);

            this.dbSet.Update(data);
            await this.context.SaveChangesAsync();
        }

        public virtual void DeleteDefinitively(Key id)
        {
            this.dbSet.Remove(dbSet.Find(id));
            this.context.SaveChanges();
        }
        public virtual async Task DeleteDefinitivelyAsync(Key id)
        {
            this.dbSet.Remove(await dbSet.FindAsync(id));
            await this.context.SaveChangesAsync();
        }

        public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> expr = null) => await this.dbSet.CountAsync(expr);
    }
}
