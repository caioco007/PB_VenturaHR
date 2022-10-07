using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IRepositoryService<TEntity, TModel, Key>
        where TEntity : class
        where TModel : class
    {
        TEntity GetDataById(Key id);
        IQueryable<TEntity> GetData();
        Task<bool> Exist(Expression<Func<TEntity, bool>> expr);


        Task<Key> CreateOrUpdateAsync(TModel model);

        Key Create(TModel model);
        void Update(TModel model);
        void Delete(Key id, int? userId = null);

        List<TModel> GetViewModel(Expression<Func<TEntity, bool>> expr = null);
        TModel GetViewModelById(Key id);
        Task<TModel> GetViewModelByIdAsync(Key id);
        TModel ToViewModel(TEntity data);

        Task<int> CountAsync(Expression<Func<TEntity, bool>> expr = null);
    }
}
