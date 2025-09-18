using Xim.Domain.Entities;
using Xim.Domain.Pagings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Xim.Library.Constants;

namespace Xim.Domain.Repos
{
    public interface IRepo<TEntity, TKey>
        where TEntity : IEntityKey<TKey>
    {
        Task<IDbConnection> OpenConnectionAsync();

        Task<bool> DeleteAsync(TKey id);
        Task<bool> Deletebyis_deleteAsync(TKey id);
        Task<bool> DeleteAsync<TData>(object id);
     
        
        Task<bool> DeleteAsyncByFieldName<TData>(object id, string fieldName);
        Task<bool> DeleteAsync(List<TKey> ids);
        Task<bool> DeleteAsync<TData>(List<object> ids);
        Task<bool> DeleteAsync<TData>(Dictionary<string, object> condition, ConditionOperator conditionOperator = ConditionOperator.And);
        Task<bool> DeleteAsync<TData>(IDbConnection cnn, Dictionary<string, object> condition, ConditionOperator conditionOperator = ConditionOperator.And);
        Task<bool> DeleteAsync<TData>(IDbTransaction tran, Dictionary<string, object> condition, ConditionOperator conditionOperator = ConditionOperator.And);

        Task<TData> InsertAsync<TData>(TData item);

        Task<TData> InsertAsync<TData>(IDbTransaction transaction, TData item);

        Task<TEntity> GetAsync(TKey id);
        Task<TData> GetAsync<TData>(object id);

        Task<List<TEntity>> GetsAsync(List<TKey> ids);
        Task<List<TData>> GetsAsync<TData>(List<object> ids);

        Task<List<TEntity>> GetsAsync(string sort = null) => GetsAsync<TEntity>(sort);
        Task<List<TData>> GetsAsync<TData>(string sort = null);

        Task<bool> UpdateAsync<TData>(TData data);

        Task<bool> UpdateAsync(TKey id, object data);
        Task<bool> UpdateAsync<TData>(object id, object data);
        Task<bool> UpdateAsync<TData>(IDbTransaction transaction, object id, object data);
        Task<bool> UpdateAsync(IDbTransaction transaction, TKey id, object data);


        Task<bool> UpdateAsync(TKey id, Dictionary<string, object> data);
        Task<bool> UpdateAsync<TData>(object id, Dictionary<string, object> data);

        Task<List<TEntity>> GetsAsync(string field, object value, string op = "=", string sort = null);
        Task<List<TData>> GetsAsync<TData>(string field, object value, string op = "=", string sort = null);

        Task<TEntity> GetAsync(string field, object value, string op = "=", string sort = null) 
            => GetAsync<TEntity>(field, value, op, sort);
        Task<TData> GetAsync<TData>(string field, object value, string op = "=", string sort = null);

        Task<TEntity> GetAsync(List<string> fields, object value, string op = "=", ConditionOperator fieldJoin = ConditionOperator.Or, string sort = null);
        Task<TData> GetAsync<TData>(List<string> fields, object value, string op = "=", ConditionOperator fieldJoin = ConditionOperator.Or, string sort = null);

        Task<List<TEntity>> GetsAsync(List<string> fields, object value, string op = "=", ConditionOperator fieldJoin = ConditionOperator.Or, string sort = null);
        Task<List<TData>> GetsAsync<TData>(List<string> fields, object value, string op = "=", ConditionOperator fieldJoin = ConditionOperator.Or, string sort = null);

        Task<TData> GetAsync<TData>(Dictionary<string, object> condition, ConditionOperator conditionOperator = ConditionOperator.And, string sort = null);
        Task<TEntity> GetAsync(Dictionary<string, object> condition, ConditionOperator conditionOperator = ConditionOperator.And, string sort = null);

        Task<List<TData>> GetsAsync<TData>(Dictionary<string, object> condition, ConditionOperator conditionOperator = ConditionOperator.And, string sort = null);
        Task<List<TEntity>> GetsAsync(Dictionary<string, object> condition, ConditionOperator conditionOperator = ConditionOperator.And, string sort = null);

        Task<IList> GetPagingAsync(PagingParam param);

        Task<object> GetPagingSummaryAsync(PagingSummaryParam param);

        Task InsertAsync<TData>(List<TData> data);

        Task InsertAsync<TData>(IDbConnection cnn, List<TData> data);

        Task InsertAsync<TData>(IDbTransaction transaction, List<TData> data);
    }
}
