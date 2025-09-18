using Newtonsoft.Json;
using Xim.Domain.Entities;
using Xim.Domain.Mssql.Models;
using Xim.Domain.Mssql.Providers;
using Xim.Domain.Mssql.Tables;
using Xim.Domain.Pagings;
using Xim.Domain.Querys;
using Xim.Domain.Repos;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xim.Library.Constants;
using System.Data.Common;

namespace Xim.Domain.Mssql.Base
{
    public abstract class MssqlRepo<TEntity, TKey> : IRepo<TEntity, TKey>
        where TEntity : IEntityKey<TKey>
    {
        #region Declaration
        protected static readonly Type ENTITY_TYPE = typeof(TEntity);
        private string _connectionString;
        private IMssqlDatabaseProvider _dbProvider;
        protected readonly IServiceProvider _serviceProvider;
        #endregion

        #region Contructor
        public MssqlRepo(
            string connection,
            IServiceProvider serviceProvider)
        {
            _connectionString = connection;
            _serviceProvider = serviceProvider;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Provider thao tác với db
        /// </summary>
        protected IMssqlDatabaseProvider Provider
        {
            get
            {
                if (_dbProvider == null)
                {
                    _dbProvider = this.CreateProvider(_connectionString);
                }
                return _dbProvider;
            }
        }
        #endregion

        #region Method

        public async Task<IDbConnection> OpenConnectionAsync()
        {
            var cnn = await this.Provider.OpenConnectionAsync();
            return cnn;
        }

        /// <summary>
        /// Khởi tạo provider thao tác với database
        /// </summary>
        /// <param name="connectionString">Chuỗi kết nối</param>
        protected virtual IMssqlDatabaseProvider CreateProvider(string connectionString)
        {
            return new MssqlProvider(connectionString, _serviceProvider);
        }

        public Task<bool> DeleteAsync(TKey id) => DeleteAsync<TEntity>((object)id);
        public Task<bool> Deletebyis_deleteAsync(TKey id) => Deletebyis_deleteAsync<TEntity>((object)id);
        public async Task<bool> Deletebyis_deleteAsync<TData>(object id)
        {
            var query = this.GetDeletebyisdeleteQuery<TData>(id);
            var res = await this.Provider.ExecuteNonQueryTextAsync(query.Query, query.Param);
            return res > 0;
        }
        public async Task<bool> DeleteAsync<TData>(object id)
        {
            var query = this.GetDeleteQuery<TData>(id);
            var res = await this.Provider.ExecuteNonQueryTextAsync(query.Query, query.Param);
            return res > 0;
        }
        public async Task<bool> DeleteAsyncByFieldName<TData>(object id, string fieldName)
        {
            var query = this.GetDeleteQueryByFieldName<TData>(id, fieldName);
            var res = await this.Provider.ExecuteNonQueryTextAsync(query.Query, query.Param);
            return res > 0;
        }

        public Task<bool> DeleteAsync(List<TKey> ids) => DeleteAsync<TEntity>(ids.Cast<object>().ToList());
        public async Task<bool> DeleteAsync<TData>(List<object> ids)
        {
            var query = this.GetDeleteQuery<TData>(ids);
            var res = await this.Provider.ExecuteNonQueryTextAsync(query.Query, query.Param);
            return res > 0;
        }

        public async Task<bool> DeleteAsync<TData>(Dictionary<string, object> condition, ConditionOperator conditionOperator = ConditionOperator.And)
        {
            var query = this.BuildDeleteByCondition<TData>(condition, conditionOperator);
            var res = await this.Provider.ExecuteNonQueryTextAsync(query, condition);
            return res > 0;
        }

        public async Task<bool> DeleteAsync<TData>(IDbConnection cnn, Dictionary<string, object> condition, ConditionOperator conditionOperator = ConditionOperator.And)
        {
            var query = this.BuildDeleteByCondition<TData>(condition, conditionOperator);
            var res = await this.Provider.ExecuteNonQueryTextAsync(cnn, query, condition);
            return res > 0;
        }
        public async Task<bool> DeleteAsync<TData>(IDbTransaction tran, Dictionary<string, object> condition, ConditionOperator conditionOperator = ConditionOperator.And)
        {
            var query = this.BuildDeleteByCondition<TData>(condition, conditionOperator);
            var res = await this.Provider.ExecuteNonQueryTextAsync(tran, query, condition);
            return res > 0;
        }

        public async Task<TData> InsertAsync<TData>(TData item)
        {
            var query = this.GetInsertQuery<TData>();
            await this.Provider.ExecuteScalarTextAsync(query, item);
            return item;
        }

        public async Task<TData> InsertAsync<TData>(IDbTransaction transaction, TData item)
        {
            var query = this.GetInsertQuery<TData>();
            await this.Provider.ExecuteScalarTextAsync(transaction, query, item);
            return item;
        }

        public Task<TEntity> GetAsync(TKey id) => GetAsync<TEntity>(id);
        public async Task<TData> GetAsync<TData>(object id)
        {
            var table = TableMap.Get(typeof(TData));
            var data = await this.GetAsync<TData>(table.KeyField, id);
            return data;
        }

        public Task<List<TEntity>> GetsAsync(List<TKey> ids)
            => GetsAsync<TEntity>(ids.Cast<object>().ToList());
        public async Task<List<TData>> GetsAsync<TData>(List<object> ids)
        {
            var table = TableMap.Get(typeof(TData));
            var data = await this.GetsAsync<TData>(table.KeyField, ids);
            return data;
        }

        public Task<List<TEntity>> GetsAsync(string sort = null)
            => GetsAsync<TEntity>(sort);
        public async Task<List<TData>> GetsAsync<TData>(string sort = null)
        {
            var table = TableMap.Get(typeof(TData));
            var sb = new StringBuilder();
            sb.Append($"SELECT * FROM {this.SafeTable(table.TableName)}");

            if (!string.IsNullOrWhiteSpace(sort))
            {
                sb.Append(" ORDER BY ")
                    .Append(this.ParseSort(sort));
            }

            var result = await this.Provider.QueryAsync<TData>(sb.ToString(), null);
            return result;
        }

        public async Task<bool> UpdateAsync<TData>(TData data)
        {
            var query = this.GetUpdateQuery<TData>(data);
            var res = await this.Provider.ExecuteNonQueryTextAsync(query, data);
            return res > 0;
        }

        public Task<bool> UpdateAsync(TKey id, object data) => UpdateAsync<TEntity>((object)id, data);
        public async Task<bool> UpdateAsync<TData>(object id, object data)
        {
            var param = new Dictionary<string, object>();
            var query = this.GetUpdateQuery<TData>(id, data, param);
            var res = await this.Provider.ExecuteNonQueryTextAsync(query, param);
            return res > 0;
        }

        public Task<bool> UpdateAsync(IDbTransaction transaction, TKey id, object data) => UpdateAsync<TEntity>(transaction, (object)id, data);
        public async Task<bool> UpdateAsync<TData>(IDbTransaction transaction, object id, object data)
        {
            var param = new Dictionary<string, object>();
            var query = this.GetUpdateQuery<TData>(id, data, param);
            var res = await this.Provider.ExecuteNonQueryTextAsync(transaction, query, param);
            return res > 0;
        }

        public Task<bool> UpdateAsync(TKey id, Dictionary<string, object> data) => UpdateAsync<TEntity>((object)id, data);
        public async Task<bool> UpdateAsync<TData>(object id, Dictionary<string, object> data)
        {
            var param = new Dictionary<string, object>();
            var query = this.GetUpdateQuery<TData>(id, data, param);
            var res = await this.Provider.ExecuteNonQueryTextAsync(query, param);
            return res > 0;
        }

        /// <summary>
        /// Gen query update
        /// </summary>
        protected virtual string GetUpdateQuery<TData>(object entity, string fields = null)
        {
            var type = typeof(TData);
            var table = TableMap.Get(type);
            var prs = type.GetProperties();
            var prKey = prs.FirstOrDefault(n => string.Equals(n.Name, table.KeyField, StringComparison.OrdinalIgnoreCase));
            if (prKey == null)
            {
                throw new NotSupportedException("Thiếu cấu hình key");
            }

            List<PropertyInfo> updateFields;
            if (string.IsNullOrEmpty(fields))
            {
                updateFields = prs.Where(n => n.Name != prKey.Name).ToList();
            }
            else
            {
                updateFields = new List<PropertyInfo>();
                foreach (var item in fields.Split(','))
                {
                    //mapping
                    foreach (var pr in prs)
                    {
                        if (pr.Name.Equals(item, StringComparison.OrdinalIgnoreCase))
                        {
                            updateFields.Add(pr);
                        }
                    }
                }
            }

            var sql = $"UPDATE {this.SafeTable(table.TableName)} SET {string.Join(",", updateFields.Select(n => $"{this.SafeColumn(n.Name)}={this.ParamField(n.Name)}"))} WHERE {this.SafeColumn(prKey.Name)}={this.ParamField(prKey.Name)};";
            return sql;
        }

        /// <summary>
        /// Gen query update
        /// </summary>
        protected virtual string GetUpdateQuery<TData>(object id, object data, Dictionary<string, object> param)
        {
            var type = typeof(TData);
            var table = TableMap.Get(type);
            var prs = type.GetProperties();
            var prKey = prs.FirstOrDefault(n => string.Equals(n.Name, table.KeyField, StringComparison.OrdinalIgnoreCase));
            if (prKey == null)
            {
                throw new NotSupportedException("Thiếu cấu hình key");
            }

            var sbField = new StringBuilder();
            var prd = data.GetType().GetProperties();
            foreach (var item in prd)
            {
                if (string.Equals(item.Name, table.KeyField, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                var map = prs.FirstOrDefault(n => string.Equals(item.Name, n.Name, StringComparison.OrdinalIgnoreCase));
                if (map == null)
                {
                    continue;
                }

                if (sbField.Length > 0)
                {
                    sbField.Append(",");
                }
                sbField.Append($"{this.SafeColumn(map.Name)}={this.ParamField(item.Name)}");
                param[item.Name] = item.GetValue(data);
            }

            param[prKey.Name] = id;
            var sql = $"UPDATE {this.SafeTable(table.TableName)} SET {sbField} WHERE {this.SafeColumn(prKey.Name)}={this.ParamField(prKey.Name)};";
            return sql;
        }

        /// <summary>
        /// Gen query update
        /// </summary>
        protected virtual string GetUpdateQuery(TKey id, Dictionary<string, object> data, Dictionary<string, object> param)
        {
            var table = TableMap.Get(ENTITY_TYPE);
            var prs = ENTITY_TYPE.GetProperties();
            var prKey = prs.FirstOrDefault(n => string.Equals(n.Name, table.KeyField, StringComparison.OrdinalIgnoreCase));
            if (prKey == null)
            {
                throw new NotSupportedException("Thiếu cấu hình key");
            }

            var sbField = new StringBuilder();
            foreach (var item in data)
            {
                if (string.Equals(item.Key, table.KeyField, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                var map = prs.FirstOrDefault(n => string.Equals(item.Key, n.Name, StringComparison.OrdinalIgnoreCase));
                if (map == null)
                {
                    continue;
                }

                if (sbField.Length > 0)
                {
                    sbField.Append(",");
                }
                sbField.Append($"{this.SafeColumn(map.Name)}={this.ParamField(item.Key)}");
                param[item.Key] = item.Value;
            }

            param[prKey.Name] = id;
            var sql = $"UPDATE {this.SafeTable(table.TableName)} SET {sbField} WHERE {this.SafeColumn(prKey.Name)}={this.ParamField(prKey.Name)};";
            return sql;
        }

        /// <summary>
        /// Gen query insert 
        /// </summary>
        protected virtual string GetInsertQuery<TData>()
        {
            var type = typeof(TData);
            var table = TableMap.Get(type);
            var prs = type.GetProperties();

            var sb = new StringBuilder($"INSERT INTO {this.SafeTable(table.TableName)} (");

            for (var i = 0; i < prs.Length; i++)
            {
                if (i > 0)
                {
                    sb.Append(",");
                }
                sb.Append(this.SafeColumn(prs[i].Name));
            }
            sb.Append(") VALUES (");

            for (var i = 0; i < prs.Length; i++)
            {
                if (i > 0)
                {
                    sb.Append(",");
                }
                sb.Append(this.ParamField(prs[i].Name));
            }
            sb.Append(");");

            return sb.ToString();
        }

        /// <summary>
        /// Gen query delete
        /// </summary>
        protected virtual SqlQuery GetDeleteQuery<TData>(object id)
        {
            var table = TableMap.Get(typeof(TData));
            var sb = new StringBuilder($"DELETE FROM {SafeTable(table.TableName)} WHERE {this.SafeColumn(table.KeyField)}");

            if (id is IList)
            {
                sb.Append(" IN ");
            }
            else
            {
                sb.Append(" = ");
            }
            sb.Append(this.ParamField(table.KeyField));

            return new SqlQuery
            {
                Query = sb.ToString(),
                Param = new Dictionary<string, object>
                {
                    { table.KeyField, id }
                }
            };
        }
        protected virtual SqlQuery GetDeletebyisdeleteQuery<TData>(object id)
        {
            var table = TableMap.Get(typeof(TData));
            var sb = new StringBuilder($"Update {SafeTable(table.TableName)} set is_deleted=1 WHERE {this.SafeColumn(table.KeyField)}");

            if (id is IList)
            {
                sb.Append(" IN ");
            }
            else
            {
                sb.Append(" = ");
            }
            sb.Append(this.ParamField(table.KeyField));

            return new SqlQuery
            {
                Query = sb.ToString(),
                Param = new Dictionary<string, object>
                {
                    { table.KeyField, id }
                }
            };
        }
        protected virtual SqlQuery GetDeleteQueryByFieldName<TData>(object id, string fieldName)
        {
            var table = TableMap.Get(typeof(TData));
            var sb = new StringBuilder($"DELETE FROM {SafeTable(table.TableName)} WHERE {this.SafeColumn(fieldName)}");

            if (id is IList)
            {
                sb.Append(" IN ");
            }
            else
            {
                sb.Append(" = ");
            }
            sb.Append(this.ParamField(table.KeyField));

            return new SqlQuery
            {
                Query = sb.ToString(),
                Param = new Dictionary<string, object>
                {
                    { table.KeyField, id }
                }
            };
        }

        /// <summary>
        /// Build câu truy vấn
        /// </summary>
        protected virtual string BuildDeleteByCondition<TData>(
            Dictionary<string, object> condition,
            ConditionOperator conditionOperator)
        {
            var joinCondition = conditionOperator == ConditionOperator.Or ? " OR" : " AND";
            var sbWhere = new StringBuilder();
            foreach (var item in condition)
            {
                if (sbWhere.Length > 0)
                {
                    sbWhere.Append(joinCondition);
                }

                sbWhere.Append($"{this.SafeColumn(item.Key)} = {this.ParamField(item.Key)}");
            }

            var table = TableMap.Get(typeof(TData));

            var sb = new StringBuilder();
            sb.Append($"DELETE FROM {this.SafeTable(table.TableName)} WHERE {sbWhere.ToString()}");

            return sb.ToString();
        }

        protected virtual string SafeTable(string table)
        {
            var sb = new StringBuilder("[");
            if (table.Contains("."))
            {
                var data = table.Split(".");
                sb.Append(this.SafeName(data.First()));
                sb.Append("].[");
                sb.Append(this.SafeName(data.Last()));
            }
            else
            {
                sb.Append(this.SafeName(table));
            }
            sb.Append("]");
            return sb.ToString();
        }

        protected virtual string SafeColumn(string column)
        {
            return $"[{this.SafeName(column)}]";
        }

        /// <summary>
        /// Xử lý chỉ chấp nhận 1 số giá trị thôi
        /// </summary>
        protected virtual string SafeName(string name)
        {
            var sb = new StringBuilder();
            foreach (char c in name)
            {
                if (c == '_'
                    || (c >= 'a' && c <= 'z')
                    || (c >= 'A' && c <= 'Z')
                    || (c >= '0' && c <= '9')
                    )
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Xử lý binding tham số
        /// </summary>
        /// <param name="field">Tên trường dữ liệu</param>
        protected virtual string ParamField(string field)
        {
            return $"@{field}";
        }

        /// <summary>
        /// Xử lý chỉ chấp nhận các toán tử hợp lệ
        /// </summary>
        /// <param name="op">Toán tử đầu vào</param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException">Nếu không hợp lệ sẽ ném về ngoại lệ</exception>
        protected virtual string SafeOperator(string op)
        {
            if (!string.IsNullOrEmpty(op))
            {
                var ops = op.Trim().ToLower();
                switch (ops)
                {
                    case "=":
                    case "<>":
                    case ">":
                    case ">=":
                    case "<":
                    case "<=":
                    case "like":
                    case "in":
                    case "not in":
                    case "between":
                        return ops;
                }
            }

            throw new NotSupportedException($"Không hỗ trợ toán tử {op}");
        }

        /// <summary>
        /// Xử lý column thành câu lệnh sql
        /// </summary>
        protected virtual string ParseColumn(string columns)
        {
            if (string.IsNullOrWhiteSpace(columns) || columns == "*")
            {
                return "*";
            }

            var sb = new StringBuilder();
            foreach (var item in columns.Split(","))
            {
                if (string.IsNullOrWhiteSpace(item))
                {
                    continue;
                }

                if (sb.Length > 0)
                {
                    sb.Append(",");
                }

                sb.Append(this.SafeColumn(item));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Build câu truy vấn
        /// </summary>
        protected virtual string BuildSelectByFieldQuery<TData>(
            Dictionary<string, object> param,
            string field,
            object value,
            string op = "=",
            string columns = "*",
            string sort = null,
            int? recordLimit = null)
        {
            var sop = this.SafeOperator(op);
            var table = TableMap.Get(typeof(TData));
            var cols = this.ParseColumn(columns);
            var sfield = this.SafeColumn(field);
            var sb = new StringBuilder($"SELECT");

            if (recordLimit > 0)
            {
                sb.Append($" TOP {recordLimit}");
            }
            sb.Append($" {cols} FROM {this.SafeTable(table.TableName)} WHERE {sfield}");

            if (value == null)
            {
                if (op == "!=")
                {
                    sb.Append(" IS NOT NULL");
                }
                else
                {
                    sb.Append(" IS NULL");
                }
            }
            else if (sop == "in" || sop == "not in")
            {
                sb.Append($" {sop} ");
                //var vl = (IList)value;
                //sb.Append("(");
                //for (var i = 0; i < vl.Count; i++)
                //{
                //    if (i > 0)
                //    {
                //        sb.Append(",");
                //    }
                //    var p = $"p{i}";
                //    sb.Append(this.ParamField(p));
                //    param[p] = vl[i];
                //}
                //sb.Append(")");
                sb.Append(this.ParamField(field));
                param[field] = value;
            }
            else
            {
                sb.Append($" {sop} ");
                sb.Append(this.ParamField(field));
                param[field] = value;
            }

            if (!string.IsNullOrWhiteSpace(sort))
            {
                sb.Append(" ORDER BY ")
                    .Append(this.ParseSort(sort));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Build câu truy vấn
        /// </summary>
        protected virtual string BuildSelectByFieldQuery<TData>(
            Dictionary<string, object> param,
            List<string> fields,
            object value,
            string op = "=",
            ConditionOperator fieldJoin = ConditionOperator.Or,
            string columns = "*",
            string sort = null,
            int? recordLimit = null)
        {
            var sop = this.SafeOperator(op);
            var table = TableMap.Get(typeof(TData));
            var cols = this.ParseColumn(columns);

            var paramField = "p";
            param[paramField] = value;
            var conditionJoin = fieldJoin.ToString().ToUpper();
            var sbWhere = new StringBuilder();
            foreach (var item in fields)
            {
                if (string.IsNullOrWhiteSpace(item))
                {
                    continue;
                }

                if (sbWhere.Length > 0)
                {
                    sbWhere.Append($" {conditionJoin} ");
                }
                sbWhere.Append($"{this.SafeColumn(item)} {sop} {this.ParamField(paramField)}");
            }

            var sb = new StringBuilder($"SELECT");
            if (recordLimit > 0)
            {
                sb.Append($" TOP {recordLimit}");
            }

            sb.Append($" {cols} FROM {this.SafeTable(table.TableName)} WHERE ({sbWhere})");

            if (!string.IsNullOrWhiteSpace(sort))
            {
                sb.Append(" ORDER BY ")
                    .Append(this.ParseSort(sort));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Build câu truy vấn
        /// </summary>
        protected virtual string BuildSelectByFieldQuery<TData>(
            Dictionary<string, object> condition,
            ConditionOperator conditionOperator,
            string sort = null,
            int? recordLimit = null)
        {
            var joinCondition = conditionOperator == ConditionOperator.Or ? " OR" : " AND";
            var sbWhere = new StringBuilder();
            foreach (var item in condition)
            {
                if (sbWhere.Length > 0)
                {
                    sbWhere.Append(joinCondition);
                }

                sbWhere.Append($"{this.SafeColumn(item.Key)} = {this.ParamField(item.Key)}");
            }

            var table = TableMap.Get(typeof(TData));

            var sb = new StringBuilder($"SELECT ");
            if (recordLimit > 0)
            {
                sb.Append($" TOP {recordLimit}");
            }
            sb.Append($" * FROM {this.SafeTable(table.TableName)} WHERE {sbWhere.ToString()}");

            if (!string.IsNullOrWhiteSpace(sort))
            {
                sb.Append(" ORDER BY ")
                    .Append(this.ParseSort(sort));
            }

            return sb.ToString();
        }

        public Task<List<TEntity>> GetsAsync(string field, object value, string op = "=", string sort = null)
            => GetsAsync<TEntity>(field, value, op, sort: sort);
        public async Task<List<TData>> GetsAsync<TData>(string field, object value, string op = "=", string sort = null)
        {
            var param = new Dictionary<string, object>();
            var sql = this.BuildSelectByFieldQuery<TData>(param, field, value, op: op, sort: sort);
            var result = await this.Provider.QueryAsync<TData>(sql, param);
            return result;
        }

        public Task<TEntity> GetAsync(string field, object value, string op = "=", string sort = null)
            => GetAsync<TEntity>(field, value, op, sort: sort);
        public async Task<TData> GetAsync<TData>(string field, object value, string op = "=", string sort = null)
        {
            var param = new Dictionary<string, object>();
            var sql = this.BuildSelectByFieldQuery<TData>(param, field, value, op: op, sort: sort, recordLimit: 1);
            var result = await this.Provider.QueryAsync<TData>(sql, param);
            return result.FirstOrDefault();
        }

        public Task<TEntity> GetAsync(List<string> fields, object value, string op = "=", ConditionOperator fieldJoin = ConditionOperator.Or, string sort = null)
            => GetAsync<TEntity>(fields, value, op, fieldJoin, sort: sort);
        public async Task<TData> GetAsync<TData>(List<string> fields, object value, string op = "=", ConditionOperator fieldJoin = ConditionOperator.Or, string sort = null)
        {
            var param = new Dictionary<string, object>();
            var sql = this.BuildSelectByFieldQuery<TData>(param, fields, value, op: op, fieldJoin: fieldJoin, sort: sort, recordLimit: 1);
            var result = await this.Provider.QueryAsync<TData>(sql, param);
            return result.FirstOrDefault();
        }

        public Task<List<TEntity>> GetsAsync(List<string> fields, object value, string op = "=", ConditionOperator fieldJoin = ConditionOperator.Or, string sort = null)
            => GetsAsync<TEntity>(fields, value, op, fieldJoin, sort: sort);
        public async Task<List<TData>> GetsAsync<TData>(List<string> fields, object value, string op = "=", ConditionOperator fieldJoin = ConditionOperator.Or, string sort = null)
        {
            var param = new Dictionary<string, object>();
            var sql = this.BuildSelectByFieldQuery<TData>(param, fields, value, op: op, sort: sort, fieldJoin: fieldJoin);
            var result = await this.Provider.QueryAsync<TData>(sql, param);
            return result;
        }

        public Task<List<TEntity>> GetsAsync(Dictionary<string, object> condition, ConditionOperator conditionOperator = ConditionOperator.And, string sort = null)
            => GetsAsync<TEntity>(condition, conditionOperator, sort: sort);
        public async Task<List<TData>> GetsAsync<TData>(Dictionary<string, object> condition, ConditionOperator conditionOperator = ConditionOperator.And, string sort = null)
        {
            var sql = this.BuildSelectByFieldQuery<TData>(condition, conditionOperator, sort: sort);
            var result = await this.Provider.QueryAsync<TData>(sql, condition);
            return result;
        }

        public Task<TEntity> GetAsync(Dictionary<string, object> condition, ConditionOperator conditionOperator = ConditionOperator.And, string sort = null)
            => GetAsync<TEntity>(condition, conditionOperator, sort: sort);
        public async Task<TData> GetAsync<TData>(Dictionary<string, object> condition, ConditionOperator conditionOperator = ConditionOperator.And, string sort = null)
        {
            var sql = this.BuildSelectByFieldQuery<TData>(condition, conditionOperator, sort: sort, recordLimit: 1);
            var result = await this.Provider.QueryAsync<TData>(sql, condition);
            return result.FirstOrDefault();
        }

        public async Task<IList> GetPagingAsync(PagingParam param)
        {
            var (query, queryParam) = this.BuildPagingDataQuery(param);

            var data = await this.Provider.QueryAsync(query, queryParam);
            return data;
        }

        /// <summary>
        /// Tạo truy vấn lấy dữ liệu paging
        /// </summary>
        protected virtual (string, Dictionary<string, object>) BuildPagingDataQuery(PagingParam param)
        {
            var table = this.GetViewListTable();
            var columnSql = this.ParseColumn(param.columns);
            var sortSql = this.ParseSort(param.sort);
            var (whereClause, whereParam) = this.ParseWhere(param.filter);
            var takeSelect = this.ProcessTake(param.take);

            var sb = new StringBuilder($"SELECT {columnSql} FROM {table}");
            if (!string.IsNullOrWhiteSpace(whereClause))
            {
                sb.Append($" WHERE {whereClause}");
            }

            if (!string.IsNullOrEmpty(sortSql))
            {
                sb.Append($" ORDER BY {sortSql}");
            }
            else
            {
                var tableConfig = TableMap.Get(ENTITY_TYPE);
                sb.Append($" ORDER BY {this.SafeColumn(tableConfig.KeyField)}");
            }

            if (param.skip > 0)
            {
                sb.Append($" OFFSET {param.skip} ROWS");
            }
            else
            {
                sb.Append($" OFFSET 0 ROWS");
            }
            sb.Append($" FETCH NEXT {takeSelect} ROWS ONLY");

            return (sb.ToString(), whereParam);
        }

        public async Task<object> GetPagingSummaryAsync(PagingSummaryParam param)
        {
            var (query, queryParam, totalField) = this.BuildPagingSummaryQuery(param);
            var sumResult = await this.Provider.QueryAsync(query, queryParam);
            return sumResult[0];
        }

        /// <summary>
        /// Tạo truy vấn lấy dữ liệu paging
        /// </summary>
        protected virtual (string, Dictionary<string, object>, string) BuildPagingSummaryQuery(PagingSummaryParam param)
        {
            var table = this.GetViewListTable();
            var totalField = "total";
            var columnSql = this.ParseSummaryColumn(param.sumColumns);
            var (whereClause, whereParam) = this.ParseWhere(param.filter);

            var sb = new StringBuilder($"SELECT COUNT(*) AS {this.SafeColumn(totalField)}");
            if (!string.IsNullOrWhiteSpace(columnSql))
            {
                sb.Append($",{columnSql}");
            }

            sb.Append($" FROM {table}");

            if (!string.IsNullOrWhiteSpace(whereClause))
            {
                sb.Append($" WHERE {whereClause}");
            }

            return (sb.ToString(), whereParam, totalField);
        }

        /// <summary>
        /// xử lý sum column thành câu lệnh sql
        /// </summary>
        protected virtual string ParseSummaryColumn(string columns)
        {
            if (string.IsNullOrWhiteSpace(columns))
            {
                return "";
            }

            var sb = new StringBuilder();
            foreach (var item in columns.Split(","))
            {
                if (this.CheckIgnoreColumn(item))
                {
                    continue;
                }

                if (sb.Length > 0)
                {
                    sb.Append(",");
                }

                //Full format <field>:<operator>:<alias>
                var splitData = item.Split(":");
                var field = this.SafeColumn(splitData[0]);
                var op = "SUM";
                if (splitData.Length > 1 && !string.IsNullOrEmpty(splitData[1]))
                {
                    switch (splitData[1].ToLower())
                    {
                        case "min":
                        case "max":
                        case "avg":
                        case "sum":
                            op = splitData[1].ToUpper();
                            break;
                    }
                }
                var alias = field;
                if (splitData.Length > 2 && !string.IsNullOrEmpty(splitData[2]))
                {
                    alias = this.SafeColumn(splitData[2]);
                }

                sb.Append($"{op}({field}) AS {alias}");
            }
            return sb.ToString();
        }

        /// <summary>
        /// Kiểm tra xem có xử lý column này không
        /// </summary>
        protected virtual bool CheckIgnoreColumn(string column)
        {
            if (string.IsNullOrWhiteSpace(column))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Lấy tên bảng/view dùng để load dữ liệu cho hàm paging
        /// </summary>
        protected virtual string GetViewListTable()
        {
            var tableConfig = TableMap.Get(ENTITY_TYPE);
            if (tableConfig == null)
            {
                throw new NotSupportedException($"Thiếu cấu hình TableMap lớp {ENTITY_TYPE.FullName}");
            }

            var table = tableConfig.ViewPaging ?? tableConfig.TableName;
            if (string.IsNullOrEmpty(table))
            {
                throw new NotSupportedException("Thiếu cấu hình TableName - ViewPaging");
            }

            return this.SafeTable(table);
        }

        /// <summary>
        /// xử lý khi không truyên hoặc truyền sai take
        /// Việc này sẽ giới hạn số lượng dữ liệu k trả về nhiều quá
        /// </summary>
        protected virtual int ProcessTake(int take)
        {
            if (take <= 0)
            {
                //default
                return 20;
            }

            if (take > 500)
            {
                //max
                return 500;
            }

            return take;
        }

        /// <summary>
        /// xử lý sort thành câu lệnh sql
        /// </summary>
        protected virtual string ParseSort(string sorts)
        {
            if (string.IsNullOrWhiteSpace(sorts))
            {
                return "";
            }

            var sb = new StringBuilder();
            foreach (var sort in sorts.Split(","))
            {
                if (string.IsNullOrWhiteSpace(sort))
                {
                    continue;
                }

                var item = sort.Trim();
                if (sb.Length > 0)
                {
                    sb.Append(",");
                }

                var ix = item.LastIndexOf(" ");
                string field;
                var dir = "ASC";
                if (ix > 0)
                {
                    field = item.Substring(0, ix).Trim();
                    var temp = item.Substring(ix + 1);
                    if ("DESC".Equals(temp, StringComparison.OrdinalIgnoreCase))
                    {
                        dir = "DESC";
                    }
                }
                else
                {
                    field = item;
                }

                sb.Append($"{this.SafeColumn(field)} {dir}");
            }

            return sb.ToString();
        }

        /// <summary>
        /// xử lý filter thành câu lệnh sql
        /// </summary>
        protected virtual (string, Dictionary<string, object>) ParseWhere(string filter)
        {
            if (string.IsNullOrWhiteSpace(filter))
            {
                return ("", null);
            }
            var items = JsonConvert.DeserializeObject<List<MssqlFilterItem>>(filter);
            var param = new Dictionary<string, object>();
            var sb = new StringBuilder();
            foreach (var item in items)
            {
                var sql = this.ParseFilter(item, param);
                if (string.IsNullOrEmpty(sql))
                {
                    continue;
                }

                if (sb.Length > 0)
                {
                    sb.Append(" AND ");
                }
                sb.Append(sql);
            }

            return (sb.ToString(), param);
        }

        /// <summary>
        /// xử lý item
        /// </summary>
        protected virtual string ParseFilter(MssqlFilterItem filter, Dictionary<string, object> param)
        {
            var sb = new StringBuilder();

            var hasOr = filter.Ors != null && filter.Ors.Count > 0;
            if (hasOr)
            {
                sb.Append("(");
            }

            sb.Append(this.SafeColumn(filter.Field));
            var op = string.IsNullOrWhiteSpace(filter.Operator) ? "=" : filter.Operator.Trim().ToUpper();
            var pname = $"f{param.Count}";
            switch (op)
            {
                case FilterOperator.Equals:
                case FilterOperator.GreaterThan:
                case FilterOperator.GreaterThanEquals:
                case FilterOperator.LessThan:
                case FilterOperator.LessThanEquals:
                case FilterOperator.NotEquals:
                    sb.Append($" {op} {this.ParamField(pname)}");
                    param[pname] = this.GetFilterValue(filter.Field, filter.Value);
                    break;
                case FilterOperator.Contains:
                    sb.Append($" LIKE {this.ParamField(pname)}");
                    param[pname] = $"%{this.GetFilterValue(filter.Field, filter.Value, op)}%";
                    break;
                case FilterOperator.Notcontains:
                    sb.Append($" NOT LIKE {this.ParamField(pname)}");
                    param[pname] = $"%{this.GetFilterValue(filter.Field, filter.Value, op)}%";
                    break;
                case FilterOperator.EndsWith:
                    sb.Append($" LIKE {this.ParamField(pname)}");
                    param[pname] = $"%{this.GetFilterValue(filter.Field, filter.Value, op)}";
                    break;
                case FilterOperator.StartsWith:
                    sb.Append($" LIKE {this.ParamField(pname)}");
                    param[pname] = $"{this.GetFilterValue(filter.Field, filter.Value, op)}%";
                    break;
                case FilterOperator.NotEndsWith:
                    sb.Append($" NOT LIKE {this.ParamField(pname)}");
                    param[pname] = $"%{this.GetFilterValue(filter.Field, filter.Value, op)}";
                    break;
                case FilterOperator.NotStartsWith:
                    sb.Append($" NOT LIKE {this.ParamField(pname)}");
                    param[pname] = $"{this.GetFilterValue(filter.Field, filter.Value, op)}%";
                    break;
                case FilterOperator.In:
                case FilterOperator.NotIn:
                    sb.Append($" {op} {this.ParamField(pname)}");
                    param[pname] = this.GetFilterValue(filter.Field, filter.Value);
                    break;
                default:
                    return null;
            }

            if (hasOr)
            {
                foreach (var item in filter.Ors)
                {
                    var sql = this.ParseFilter(item, param);
                    if (string.IsNullOrWhiteSpace(sql))
                    {
                        continue;
                    }

                    sb.Append($" OR {sql}");
                }

                sb.Append(")");
            }
            return sb.ToString();
        }

        /// <summary>
        /// Đọc dữ liệu filter về đúng kiểu
        /// </summary>
        /// <summary>
        /// Đọc dữ liệu filter về đúng kiểu
        /// </summary>
        protected virtual object GetFilterValue(string field, object value, string op = null)
        {
            if (value is string valueStr)
            {
                DateTime tempDate;
                //nameing convention
                if (field.Contains("time", StringComparison.OrdinalIgnoreCase) && DateTime.TryParseExact(value as string, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out tempDate))
                {
                    return tempDate;
                }
                if (field.Contains("date", StringComparison.OrdinalIgnoreCase) && DateTime.TryParseExact(value as string, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out tempDate))
                {
                    return tempDate;
                }

                valueStr = ProcessFilterValue(valueStr, op);
                return valueStr;
            }

            return value;
        }

        /// <summary>
		/// Xử lý replace các kí tự đặc biệt khi sử dụng toán tử like trong Mssql
		/// </summary>
		/// <param name="valueStr">Value dạng string</param>
		/// <param name="op">Toán tử</param>
		/// <returns></returns>
		protected virtual string ProcessFilterValue(string valueStr, string op)
        {
            var specialCharacterQuery = new char[] { '\\', '%', '_', '\"', '\'' };
            var reg = "[\"\\\\'%_]";
            switch (op)
            {
                case FilterOperator.Contains:
                case FilterOperator.Notcontains:
                case FilterOperator.StartsWith:
                case FilterOperator.NotStartsWith:
                case FilterOperator.EndsWith:
                case FilterOperator.NotEquals:
                    valueStr = Regex.Replace(valueStr, reg, (match) =>
                    {
                        return $"\\{match.Value}";
                    });
                    break;

                default:
                    break;
            }
            return valueStr;
        }

        public virtual async Task InsertAsync<TData>(List<TData> data)
        {
            var querys = this.BuildInsertBatchQuery(data);
            using (var cnn = await this.OpenConnectionAsync())
            {
                foreach (var item in querys)
                {
                    await this.Provider.ExecuteNonQueryTextAsync(cnn, item.Query, item.Param);
                }
            }
        }

        public virtual async Task InsertAsync<TData>(IDbConnection cnn, List<TData> data)
        {
            var querys = this.BuildInsertBatchQuery(data);
            foreach (var item in querys)
            {
                await this.Provider.ExecuteNonQueryTextAsync(cnn, item.Query, item.Param);
            }
        }

        public virtual async Task InsertAsync<TData>(IDbTransaction transaction, List<TData> data)
        {
            var querys = this.BuildInsertBatchQuery(data);
            foreach (var item in querys)
            {
                await this.Provider.ExecuteNonQueryTextAsync(transaction, item.Query, item.Param);
            }
        }

        /// <summary>
        /// Build query insert batch
        /// </summary>
        /// <param name="data">Dữ liệu</param>
        protected List<SqlQuery> BuildInsertBatchQuery<TData>(List<TData> data)
        {
            var result = new List<SqlQuery>();
            if (data.Count == 0)
            {
                return result;
            }

            const int LIMIT_PARAM = 100;
            var type = typeof(TData);
            var table = TableMap.Get(type);
            var prs = type.GetProperties();

            var insertPrefix = $"INSERT INTO {this.SafeTable(table.TableName)} ({string.Join(",", prs.Select(n => this.SafeColumn(n.Name)))}) VALUES";
            SqlQuery query = null;
            Dictionary<object, string> valueMap = null;
            StringBuilder sb = null;

            int paramIndex = 0;
            for (var i = 0; i < data.Count; i++)
            {
                //đạt limit => tạo query mới
                if (i == 0
                    || query.Param.Count + prs.Length > LIMIT_PARAM)
                {
                    //build query
                    if (i > 0)
                    {
                        query.Query = sb.ToString();
                    }

                    query = new SqlQuery()
                    {
                        Param = new Dictionary<string, object>()
                    };
                    valueMap = new Dictionary<object, string>();
                    sb = new StringBuilder(insertPrefix);
                    result.Add(query);
                }

                if (i == 0 || query.Param.Count == 0)
                {
                    sb.Append("(");
                }
                else
                {
                    sb.Append(",(");
                }

                var item = data[i];
                for (var j = 0; j < prs.Length; j++)
                {
                    if (j > 0)
                    {
                        sb.Append(",");
                    }
                    var pr = prs[j];
                    var field = pr.Name;
                    var value = pr.GetValue(item);

                    if (value == null)
                    {
                        field = "pnu";
                    }
                    else if (valueMap.ContainsKey(value))
                    {
                        field = valueMap[value];
                    }
                    else
                    {
                        field = $"p{paramIndex++}";
                        valueMap[value] = field;
                    }
                    sb.Append(this.ParamField(field));
                    query.Param[field] = value;
                }
                sb.Append(")");
            }

            //last query
            query.Query = sb.ToString();

            return result;
        }
        #endregion
    }
}
