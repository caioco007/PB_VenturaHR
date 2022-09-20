using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace VenturaHR.Models
{
    public class SqlParameterList
    {
        class SqlParameterQuery
        {
            public string Key { get; set; }
            public string Query { get; set; }
            public SqlParameter Parameter { get; set; }
        }

        private Dictionary<string, SqlParameter> Parameters { get; set; }
        private List<SqlParameterQuery> Queries { get; set; }

        public SqlParameterList()
        {
            Parameters = new Dictionary<string, SqlParameter>();
            Queries = new List<SqlParameterQuery>();
        }

        public void AddParameter(string key, object valor)
        {
            if (Parameters.ContainsKey(key))
            {
                Parameters.Remove(key);
            }

            Parameters.Add(key, new SqlParameter($"@{key}", valor ?? (object)DBNull.Value));
        }
        public void RemoveParameter(string key)
        {
            if (!Parameters.ContainsKey(key)) return;

            Parameters.Remove(key);
        }

        public void AddQuery(string key, string query, object valor)
        {
            if (Queries.Any(x => x.Key == key))
                Queries.Remove(Queries.First(x => x.Key == key));

            Queries.Add(new SqlParameterQuery { Key = key, Query = query, Parameter = new SqlParameter($"@{key}", valor ?? (object)DBNull.Value) });
        }
        public void RemoveQuery(string key)
        {
            if (!Queries.Any(x => x.Key == key)) return;

            Queries.Remove(Queries.First(x => x.Key == key));
        }

        public SqlParameter[] GetParameters() => Parameters.Values.Union(Queries.Select(x => x.Parameter)).ToArray();
        public string GetQuery() => string.Join(" AND ", Parameters.Keys.Select(x => $"{x} = @{x}").Union(Queries.Select(x => x.Query)));
    }
}
