using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Shared
{
    public static class RepositoryServiceDataTableExtensions
    {
        public static IQueryable<TEntity> GetDataFiltered<TEntity, TModel, Key>(this RepositoryService<TEntity, TModel, Key> me, DTO.Shared.DataTablesAjaxPostModel filter, out int recordsTotal, out int recordsFiltered) where TEntity : class where TModel : class
        {
            return GetDataFiltered(me, filter, out recordsTotal, out recordsFiltered, null, new SqlParameter[] { });
        }
        public static IQueryable<TEntity> GetDataFiltered<TEntity, TModel, Key>(this RepositoryService<TEntity, TModel, Key> me, DTO.Shared.DataTablesAjaxPostModel filter, out int recordsTotal, out int recordsFiltered, string whereSQL, SqlParameter whereParameter) where TEntity : class where TModel : class
        {
            if (whereParameter == null)
                return GetDataFiltered(me, filter, out recordsTotal, out recordsFiltered, whereSQL, whereParameters: null);
            else
                return GetDataFiltered(me, filter, out recordsTotal, out recordsFiltered, whereSQL, new SqlParameter[] { whereParameter });
        }
        public static IQueryable<TEntity> GetDataFiltered<TEntity, TModel, Key>(this RepositoryService<TEntity, TModel, Key> me, DTO.Shared.DataTablesAjaxPostModel filter, out int recordsTotal, out int recordsFiltered, string whereSQL, SqlParameter[] whereParameters) where TEntity : class where TModel : class
        {
            var sql = new StringBuilder();
            var guid = Guid.NewGuid().ToString("D");
            var parameters = new List<SqlParameter>();

            #region [WHERE PARAMETERS]
            if (whereParameters != null)
                foreach (var whereParameter in whereParameters)
                    parameters.Add(whereParameter);
            #endregion

            #region [TOTAL COUNT]
            recordsFiltered = 1;
            sql.AppendFormat("SELECT {0} FROM [{1}].[{2}](NOLOCK) ", guid, me.schema, me.table);
            using (var command = me.context.Database.GetDbConnection().CreateCommand())
            {
                me.context.Database.OpenConnection();
                if (string.IsNullOrWhiteSpace(whereSQL))
                {
                    command.CommandText = string.Format("SELECT COUNT(*) FROM [{0}].[{1}](NOLOCK)", me.schema, me.table);
                }
                else
                {
                    command.Parameters.AddRange(parameters.ToArray());
                    command.CommandText = string.Format("SELECT COUNT(*) FROM [{0}].[{1}](NOLOCK) WHERE " + whereSQL, me.schema, me.table);
                }
                recordsTotal = (int)command.ExecuteScalar();
                command.Parameters.Clear();
            }
            #endregion

            #region [APPLY COLUMN FILTER]
            bool hasColumnFilter = false;
            #region [CUSTOM WHERE]
            if (string.IsNullOrWhiteSpace(whereSQL))
                sql.Append("WHERE 1 = 1 AND ( ");
            else
                sql.Append("WHERE (" + whereSQL + ") AND ( ");
            #endregion

            if (!string.IsNullOrWhiteSpace(filter.search.value))
            {
                foreach (var q in filter.search.value.Split(','))
                {
                    sql.Append("(");
                    foreach (var c in filter.columns)
                    {
                        if (string.IsNullOrWhiteSpace(c.data)) continue;
                        if (typeof(TEntity).GetProperty(c.data) == null) continue;

                        var parameterGuid = "PARAM_" + Guid.NewGuid().ToString("N");
                        sql.Append("(" + c.data + " LIKE @" + parameterGuid + ") OR ");
                        parameters.Add(new SqlParameter("@" + parameterGuid, "%" + q.Trim() + "%"));

                    }
                    sql = new StringBuilder(sql.ToString().Remove(sql.ToString().LastIndexOf("OR "), 3));
                    sql.Append(") AND ");
                    hasColumnFilter = true;
                }
            }

            if (!hasColumnFilter)
                sql = new StringBuilder(sql.ToString().Remove(sql.ToString().LastIndexOf("AND ( "), 6));
            else
            {
                sql = new StringBuilder(sql.ToString().Remove(sql.ToString().LastIndexOf("AND "), 4));
                sql.Append(") ");
            }
            #endregion

            #region [FILTER COUNT]
            using (var command = me.context.Database.GetDbConnection().CreateCommand())
            {
                me.context.Database.OpenConnection();
                command.CommandText = sql.ToString().Replace(guid, "COUNT(*)");
                command.Parameters.AddRange(parameters.ToArray());
                recordsFiltered = (int)command.ExecuteScalar();
                command.Parameters.Clear();
            }
            #endregion

            #region [ORDER]
            sql.Append("ORDER BY ");
            bool isOrdered = false;
            foreach (var o in filter.order)
            {
                var columnName = filter.columns[o.column].data;
                if (string.IsNullOrWhiteSpace(columnName)) continue;
                if (typeof(TEntity).GetProperty(columnName) == null) continue;

                sql.AppendFormat(" [{0}] {1}, ", columnName, o.dir.ToUpper());
                isOrdered = true;
            }
            if (!isOrdered)
                sql.Append("1 ");
            else
                sql = new StringBuilder(sql.ToString().Remove(sql.ToString().LastIndexOf(","), 1));
            #endregion

            #region [PAGGING]
            if (filter.start.HasValue)
                sql.AppendFormat("OFFSET {0} ROWS ", (filter.start));
            else if (filter.length.HasValue)
                sql.Append("OFFSET 0 ROWS ");

            if (filter.length.HasValue) sql.AppendFormat("FETCH NEXT {0} ROWS ONLY ", filter.length);
            #endregion

            return me.dbSet.FromSqlRaw<TEntity>(sql.ToString().Replace(guid, "*"), parameters.ToArray());
        }
    }
}
