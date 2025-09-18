using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using System.Data.SqlClient;

namespace Xim.Domain.Mssql.Providers
{
    /// <summary>
    /// Thao tácvới database
    /// </summary>
    public class MssqlProvider : IMssqlDatabaseProvider
    {
        /// <summary>
        /// các từ check sql injection
        /// </summary>
        private static string[] _InjectionWords = new string[] {
            "\"",
            //"'",
            "--",
            "#",
            "\\/*",
            "*\\/",
            "grant ",
            "drop ",
            "truncate ",
            "sleep(",
            "exec ",
            "execute ",
            "prepare ",
            "information_schema",
            "delay ",
        };
        /// <summary>
        /// các từ khóa bỏ qua khi kiểm tra
        /// </summary>
        private static string[] _InjectionIgnoreWords = new string[] {
            "''",
            "'MORE_DETAILS'",
            "'$'", "\"$\"",
            "'%'","\"%\"",
            "','","\",\"",
            "'\", \"'", "'\",\"'",
            "'\\[\"'",
            "'\"\\]'",
            "\"$[*]\"", @"\'$[*]\'",
            @"\'\$\.[a-zA-Z0-9]+\'", "\"\\$\\.[a-zA-Z0-9]+\"",
            @"'null'", "\"null\"",
            "\"ONLY_FULL_GROUP_BY,NO_UNSIGNED_SUBTRACTION,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION\"",
            "'Other'",
            "'#'",
            "Drop TEMPORARY table"
        };
        private static string _InjectionIgnoreWordsReplace = " ";
        /// <summary>
        /// Các từ khóa sẽ bị loại bỏ khỏi câu lệnh thực thi
        /// </summary>
        private static Dictionary<string, string> _InjectionRemoveWords = new Dictionary<string, string>
        {
            { "\\/\\*(.*)\\*\\/", ""},  //comment block
        };

        private readonly string _connectionString;
        private readonly IServiceProvider _serviceProvider;
        public MssqlProvider(
            string connectionString,
            IServiceProvider serviceProvider
            )
        {
            _connectionString = connectionString;
            _serviceProvider = serviceProvider;
        }

        //#region Store

        //#region Read data
        //public async Task<List<T>> ExecuteStoreQueryAsync<T>(string storeName, object param = null)
        //{
        //    using (var cnn = await OpenConnectionAsync())
        //    {
        //        return await this.ExecuteStoreQueryAsync<T>(cnn, storeName, param);
        //    }
        //}

        //public async Task<List<T>> ExecuteStoreQueryAsync<T>(string storeName, Dictionary<string, object> param = null)
        //{
        //    using (var cnn = await OpenConnectionAsync())
        //    {
        //        return await this.ExecuteStoreQueryAsync<T>(cnn, storeName, param);
        //    }
        //}

        //public async Task<List<T>> ExecuteStoreQueryAsync<T>(string storeName, object[] param = null)
        //{
        //    using (var cnn = await OpenConnectionAsync())
        //    {
        //        return await this.ExecuteStoreQueryAsync<T>(cnn, storeName, param);
        //    }
        //}

        //public async Task<List<T>> ExecuteStoreQueryAsync<T>(IDbConnection cnn, string storeName, object param = null)
        //{
        //    throw new NotImplementedException();
        //    //var queryParam = new StoreQueryParamObject(param);
        //    //var result = await this.ExecuteProcedureReaderAsync<T>(storeName, queryParam, cnn);
        //    //return result;
        //}

        //public async Task<List<T>> ExecuteStoreQueryAsync<T>(IDbConnection cnn, string storeName, Dictionary<string, object> param = null)
        //{
        //    throw new NotImplementedException();
        //    //var queryParam = new StoreQueryParamDictionary(param);
        //    //var result = await this.ExecuteProcedureReaderAsync<T>(storeName, queryParam, cnn);
        //    //return result;
        //}

        //public async Task<List<T>> ExecuteStoreQueryAsync<T>(IDbConnection cnn, string storeName, object[] param = null)
        //{
        //    throw new NotImplementedException();
        //    //var queryParam = new StoreQueryParamArray(param);
        //    //var result = await this.ExecuteProcedureReaderAsync<T>(storeName, queryParam, cnn);
        //    //return result;
        //}

        //public async Task<List<T>> ExecuteStoreQueryAsync<T>(IDbTransaction tran, string storeName, object param = null)
        //{
        //    throw new NotImplementedException();
        //    //var queryParam = new StoreQueryParamObject(param);
        //    //var result = await this.ExecuteProcedureReaderAsync<T>(storeName, queryParam, tran.Connection, tran: tran);
        //    //return result;
        //}

        //public async Task<List<T>> ExecuteStoreQueryAsync<T>(IDbTransaction tran, string storeName, Dictionary<string, object> param = null)
        //{
        //    throw new NotImplementedException();
        //    //var queryParam = new StoreQueryParamDictionary(param);
        //    //var result = await this.ExecuteProcedureReaderAsync<T>(storeName, queryParam, tran.Connection, tran: tran);
        //    //return result;
        //}

        //public async Task<List<T>> ExecuteStoreQueryAsync<T>(IDbTransaction tran, string storeName, object[] param = null)
        //{
        //    throw new NotImplementedException();
        //    //var queryParam = new StoreQueryParamArray(param);
        //    //var result = await this.ExecuteProcedureReaderAsync<T>(storeName, queryParam, tran.Connection, tran: tran);
        //    //return result;
        //}
        //#endregion

        //#region NonQuery

        //public async Task<int> ExecuteStoreNonQueryAsync(string storeName, object param = null)
        //{
        //    using (var cnn = await OpenConnectionAsync())
        //    {
        //        return await this.ExecuteStoreNonQueryAsync(cnn, storeName, param);
        //    }
        //}

        //public async Task<int> ExecuteStoreNonQueryAsync(string storeName, Dictionary<string, object> param = null)
        //{
        //    using (var cnn = await OpenConnectionAsync())
        //    {
        //        return await this.ExecuteStoreNonQueryAsync(cnn, storeName, param);
        //    }
        //}

        //public async Task<int> ExecuteStoreNonQueryAsync(string storeName, object[] param = null)
        //{
        //    using (var cnn = await OpenConnectionAsync())
        //    {
        //        return await this.ExecuteStoreNonQueryAsync(cnn, storeName, param);
        //    }
        //}

        //public async Task<int> ExecuteStoreNonQueryAsync(IDbConnection cnn, string storeName, object param = null)
        //{
        //    throw new NotImplementedException();
        //    //var queryParam = new StoreQueryParamObject(param);
        //    //var result = await this.ExecuteProcedureNonQueryAsync(storeName, queryParam, cnn);
        //    //return result;
        //}

        //public async Task<int> ExecuteStoreNonQueryAsync(IDbConnection cnn, string storeName, Dictionary<string, object> param = null)
        //{
        //    throw new NotImplementedException();
        //    //var queryParam = new StoreQueryParamDictionary(param);
        //    //var result = await this.ExecuteProcedureNonQueryAsync(storeName, queryParam, cnn);
        //    //return result;
        //}

        //public async Task<int> ExecuteStoreNonQueryAsync(IDbConnection cnn, string storeName, object[] param = null)
        //{
        //    throw new NotImplementedException();
        //    //var queryParam = new StoreQueryParamArray(param);
        //    //var result = await this.ExecuteProcedureNonQueryAsync(storeName, queryParam, cnn);
        //    //return result;
        //}

        //public async Task<int> ExecuteStoreNonQueryAsync(IDbTransaction tran, string storeName, object param = null)
        //{
        //    throw new NotImplementedException();
        //    //var queryParam = new StoreQueryParamObject(param);
        //    //var result = await this.ExecuteProcedureNonQueryAsync(storeName, queryParam, tran.Connection, tran: tran);
        //    //return result;
        //}

        //public async Task<int> ExecuteStoreNonQueryAsync(IDbTransaction tran, string storeName, Dictionary<string, object> param = null)
        //{
        //    throw new NotImplementedException();
        //    //var queryParam = new StoreQueryParamDictionary(param);
        //    //var result = await this.ExecuteProcedureNonQueryAsync(storeName, queryParam, tran.Connection, tran: tran);
        //    //return result;
        //}

        //public async Task<int> ExecuteStoreNonQueryAsync(IDbTransaction tran, string storeName, object[] param = null)
        //{
        //    throw new NotImplementedException();
        //    //var queryParam = new StoreQueryParamArray(param);
        //    //var result = await this.ExecuteProcedureNonQueryAsync(storeName, queryParam, tran.Connection, tran: tran);
        //    //return result;
        //}

        //#endregion

        //#region Scala
        //public async Task<T> ExecuteStoreScalarAsync<T>(string storeName, object param = null)
        //{
        //    using (var cnn = await OpenConnectionAsync())
        //    {
        //        return await this.ExecuteStoreScalarAsync<T>(cnn, storeName, param);
        //    }
        //}

        //public async Task<T> ExecuteStoreScalarAsync<T>(string storeName, Dictionary<string, object> param = null)
        //{
        //    using (var cnn = await OpenConnectionAsync())
        //    {
        //        return await this.ExecuteStoreScalarAsync<T>(cnn, storeName, param);
        //    }
        //}

        //public async Task<T> ExecuteStoreScalarAsync<T>(string storeName, object[] param = null)
        //{
        //    using (var cnn = await OpenConnectionAsync())
        //    {
        //        return await this.ExecuteStoreScalarAsync<T>(cnn, storeName, param);
        //    }
        //}


        //public async Task<T> ExecuteStoreScalarAsync<T>(IDbConnection cnn, string storeName, object param = null)
        //{
        //    throw new NotImplementedException();
        //    //var queryParam = new StoreQueryParamObject(param);
        //    //var result = await this.ExecuteProcedureScalarAsync<T>(storeName, queryParam, cnn);
        //    //return result;
        //}

        //public async Task<T> ExecuteStoreScalarAsync<T>(IDbConnection cnn, string storeName, Dictionary<string, object> param = null)
        //{
        //    throw new NotImplementedException();
        //    //var queryParam = new StoreQueryParamDictionary(param);
        //    //var result = await this.ExecuteProcedureScalarAsync<T>(storeName, queryParam, cnn);
        //    //return result;
        //}

        //public async Task<T> ExecuteStoreScalarAsync<T>(IDbConnection cnn, string storeName, object[] param = null)
        //{
        //    throw new NotImplementedException();
        //    //var queryParam = new StoreQueryParamArray(param);
        //    //var result = await this.ExecuteProcedureScalarAsync<T>(storeName, queryParam, cnn);
        //    //return result;
        //}


        //public async Task<T> ExecuteStoreScalarAsync<T>(IDbTransaction tran, string storeName, object param = null)
        //{
        //    throw new NotImplementedException();
        //    //var queryParam = new StoreQueryParamObject(param);
        //    //var result = await this.ExecuteProcedureScalarAsync<T>(storeName, queryParam, tran.Connection, tran: tran);
        //    //return result;
        //}

        //public async Task<T> ExecuteStoreScalarAsync<T>(IDbTransaction tran, string storeName, Dictionary<string, object> param = null)
        //{
        //    throw new NotImplementedException();
        //    //var queryParam = new StoreQueryParamDictionary(param);
        //    //var result = await this.ExecuteProcedureScalarAsync<T>(storeName, queryParam, tran.Connection, tran: tran);
        //    //return result;
        //}

        //public async Task<T> ExecuteStoreScalarAsync<T>(IDbTransaction tran, string storeName, object[] param = null)
        //{
        //    throw new NotImplementedException();
        //    //var queryParam = new StoreQueryParamArray(param);
        //    //var result = await this.ExecuteProcedureScalarAsync<T>(storeName, queryParam, tran.Connection, tran: tran);
        //    //return result;
        //}

        ////--
        //public async Task<object> ExecuteStoreScalarAsync(string storeName, object param = null)
        //{
        //    using (var cnn = await OpenConnectionAsync())
        //    {
        //        return await this.ExecuteStoreScalarAsync(cnn, storeName, param);
        //    }
        //}

        //public async Task<object> ExecuteStoreScalarAsync(string storeName, Dictionary<string, object> param = null)
        //{
        //    using (var cnn = await OpenConnectionAsync())
        //    {
        //        return await this.ExecuteStoreScalarAsync(cnn, storeName, param);
        //    }
        //}

        //public async Task<object> ExecuteStoreScalarAsync(string storeName, object[] param = null)
        //{
        //    using (var cnn = await OpenConnectionAsync())
        //    {
        //        return await this.ExecuteStoreScalarAsync(cnn, storeName, param);
        //    }
        //}

        //public async Task<object> ExecuteStoreScalarAsync(IDbConnection cnn, string storeName, object param = null)
        //{
        //    throw new NotImplementedException();
        //    //var queryParam = new StoreQueryParamObject(param);
        //    //var result = await this.ExecuteProcedureScalarAsync(storeName, queryParam, cnn);
        //    //return result;
        //}

        //public async Task<object> ExecuteStoreScalarAsync(IDbConnection cnn, string storeName, Dictionary<string, object> param = null)
        //{
        //    throw new NotImplementedException();
        //    //var queryParam = new StoreQueryParamDictionary(param);
        //    //var result = await this.ExecuteProcedureScalarAsync(storeName, queryParam, cnn);
        //    //return result;
        //}

        //public async Task<object> ExecuteStoreScalarAsync(IDbConnection cnn, string storeName, object[] param = null)
        //{
        //    throw new NotImplementedException();
        //    //var queryParam = new StoreQueryParamArray(param);
        //    //var result = await this.ExecuteProcedureScalarAsync(storeName, queryParam, cnn);
        //    //return result;
        //}

        //public async Task<object> ExecuteStoreScalarAsync(IDbTransaction tran, string storeName, object param = null)
        //{
        //    throw new NotImplementedException();
        //    //var queryParam = new StoreQueryParamObject(param);
        //    //var result = await this.ExecuteProcedureScalarAsync(storeName, queryParam, tran.Connection, tran: tran);
        //    //return result;
        //}

        //public async Task<object> ExecuteStoreScalarAsync(IDbTransaction tran, string storeName, Dictionary<string, object> param = null)
        //{
        //    throw new NotImplementedException();
        //    //var queryParam = new StoreQueryParamDictionary(param);
        //    //var result = await this.ExecuteProcedureScalarAsync(storeName, queryParam, tran.Connection, tran: tran);
        //    //return result;
        //}

        //public async Task<object> ExecuteStoreScalarAsync(IDbTransaction tran, string storeName, object[] param = null)
        //{
        //    throw new NotImplementedException();
        //    //var queryParam = new StoreQueryParamArray(param);
        //    //var result = await this.ExecuteProcedureScalarAsync(storeName, queryParam, tran.Connection, tran: tran);
        //    //return result;
        //}
        //#endregion

        //#endregion

        public async Task<int> ExecuteNonQueryTextAsync(string commandText, Dictionary<string, object> param, int? timeout = null)
        {
            using (var cnn = await OpenConnectionAsync())
            {
                return await this.ExecuteNonQueryTextAsync(cnn, commandText, param, timeout: timeout);
            }
        }

        public async Task<int> ExecuteNonQueryTextAsync(IDbConnection cnn, string commandText, Dictionary<string, object> param, int? timeout = null)
        {
            var sql = this.ProcessSqlBeforExecute(commandText);
            var result = await cnn.ExecuteAsync(sql, param, commandType: CommandType.Text, commandTimeout: timeout);
            return result;
        }
        public async Task<int> ExecuteNonQueryTextAsync(IDbTransaction transaction, string commandText, Dictionary<string, object> param)
        {
            var sql = this.ProcessSqlBeforExecute(commandText);
            var result = await transaction.Connection.ExecuteAsync(sql, param, commandType: CommandType.Text, transaction: transaction);
            return result;
        }

        public async Task<int> ExecuteNonQueryTextAsync(string commandText, object param)
        {
            using (var cnn = await OpenConnectionAsync())
            {
                return await this.ExecuteNonQueryTextAsync(cnn, commandText, param);
            }
        }
        public async Task<int> ExecuteNonQueryTextAsync(IDbConnection cnn, string commandText, object param)
        {
            var sql = this.ProcessSqlBeforExecute(commandText);
            var result = await cnn.ExecuteAsync(sql, param, commandType: CommandType.Text);
            return result;
        }
        public async Task<int> ExecuteNonQueryTextAsync(IDbTransaction transaction, string commandText, object param)
        {
            var sql = this.ProcessSqlBeforExecute(commandText);
            var result = await transaction.Connection.ExecuteAsync(sql, param, commandType: CommandType.Text, transaction: transaction);
            return result;
        }
        public async Task<object> ExecuteScalarTextAsync(IDbTransaction transaction, string commandText, object param)
        {
            var sql = this.ProcessSqlBeforExecute(commandText);
            var result = await transaction.Connection.ExecuteScalarAsync(sql, param, commandType: CommandType.Text, transaction: transaction);
            return result;
        }
        public async Task<object> ExecuteScalarTextAsync(string commandText, Dictionary<string, object> param)
        {
            using (var cnn = await OpenConnectionAsync())
            {
                return await this.ExecuteScalarTextAsync(cnn, commandText, param);
            }
        }
        public async Task<object> ExecuteScalarTextAsync(IDbConnection cnn, string commandText, Dictionary<string, object> param)
        {
            var sql = this.ProcessSqlBeforExecute(commandText);
            var result = await cnn.ExecuteScalarAsync(sql, param, commandType: CommandType.Text);
            return result;
        }
        public async Task<object> ExecuteScalarTextAsync(IDbTransaction transaction, string commandText, Dictionary<string, object> param)
        {
            var sql = this.ProcessSqlBeforExecute(commandText);
            var result = await transaction.Connection.ExecuteScalarAsync(sql, param, commandType: CommandType.Text, transaction: transaction);
            return result;
        }
        public async Task<object> ExecuteScalarTextAsync(string commandText, object param)
        {
            using (var cnn = await OpenConnectionAsync())
            {
                return await this.ExecuteScalarTextAsync(cnn, commandText, param);
            }
        }
        public async Task<object> ExecuteScalarTextAsync(IDbConnection cnn, string commandText, object param)
        {
            var sql = this.ProcessSqlBeforExecute(commandText);
            var result = await cnn.ExecuteScalarAsync(sql, param, commandType: CommandType.Text);
            return result;
        }

        public async Task<List<T>> QueryAsync<T>(string commandText, Dictionary<string, object> param)
        {
            using (var cnn = await OpenConnectionAsync())
            {
                return await this.QueryAsync<T>(cnn, commandText, param);
            }
        }

        public async Task<List<T>> QueryAsync<T>(IDbConnection cnn, string commandText, Dictionary<string, object> param)
        {
            var sql = this.ProcessSqlBeforExecute(commandText);
            var result = await cnn.QueryAsync<T>(sql, param, commandType: CommandType.Text);
            return result.AsList();
        }
        public List<T> Query<T>(IDbConnection cnn, string commandText, Dictionary<string, object> param)
        {
            var sql = this.ProcessSqlBeforExecute(commandText);
            var result = cnn.Query<T>(sql, param, commandType: CommandType.Text);
            return result.AsList();
        }
        public async Task<List<T>> QueryAsync<T>(IDbTransaction transaction, string commandText, Dictionary<string, object> param)
        {
            var sql = this.ProcessSqlBeforExecute(commandText);
            var result = await transaction.Connection.QueryAsync<T>(sql, param, commandType: CommandType.Text, transaction: transaction);
            return result.AsList();
        }

        public async Task<IList> QueryAsync(Type type, string commandText, Dictionary<string, object> param)
        {
            using (var cnn = await OpenConnectionAsync())
            {
                return await this.QueryAsync(cnn, type, commandText, param);
            }
        }
        public async Task<IList> QueryAsync(IDbTransaction transaction, Type type, string commandText, Dictionary<string, object> param)
        {
            var sql = this.ProcessSqlBeforExecute(commandText);
            var data = await transaction.Connection.QueryAsync(type, sql, param, commandType: CommandType.Text, transaction: transaction) as IList;
            var result = ConvertResult(data, type);
            return result;
        }

        public async Task<IList> QueryAsync(IDbConnection cnn, Type type, string commandText, Dictionary<string, object> param)
        {
            var sql = this.ProcessSqlBeforExecute(commandText);
            var data = await cnn.QueryAsync(type, sql, param, commandType: CommandType.Text) as IList;
            var result = ConvertResult(data, type);
            return result;
        }

        public async Task<IList> QueryAsync(string commandText, Dictionary<string, object> param)
        {
            using (var cnn = await OpenConnectionAsync())
            {
                return await this.QueryAsync(cnn, commandText, param);
            }
        }

        public async Task<IList> QueryAsync(IDbConnection cnn, string commandText, Dictionary<string, object> param)
        {
            var sql = this.ProcessSqlBeforExecute(commandText);
            var data = await cnn.QueryAsync(sql, param, commandType: CommandType.Text);
            return data.ToList();
        }

        public async Task<List<T>> QueryWithCommandTypeAsync<T>(IDbConnection cnn, string commandText, CommandType pCommandType, Dictionary<string, object> param)
        {
            var sql = this.ProcessSqlBeforExecute(commandText);
            var data = await cnn.QueryAsync<T>(sql, param, commandType: pCommandType);
            return data.ToList();
        }

        public async Task<List<T>> QueryWithCommandTypeWithTranAsync<T>(IDbTransaction transaction, string commandText, CommandType pCommandType, Dictionary<string, object> param)
        {
            var sql = this.ProcessSqlBeforExecute(commandText);
            var data = await transaction.Connection.QueryAsync<T>(sql, param, commandType: pCommandType);
            return data.ToList();
        }

        #region Private method

        /// <summary>
        /// Chuyển vỏ ilist về đúng kiểu
        /// </summary>
        private IList ConvertResult(IList data, Type type)
        {
            var result = CreateList(type);
            foreach (var item in data)
            {
                result.Add(item);
            }

            return result;
        }

        ///// <summary>
        ///// Thực thi thủ tục trả về row effect
        ///// </summary>
        //private async Task<int> ExecuteProcedureNonQueryAsync(string storeName, IStoreQueryParam queryParam, IDbConnection cnn, IDbTransaction tran = null)
        //{
        //    //Kiểm tra sqlinjection
        //    this.ValidateStore(storeName);

        //    //lấy thông tin thủ tục
        //    var storeInfo = this.GetProcedureInfo(cnn, storeName);

        //    //build câu lệnh gọi db
        //    var query = this.BuildProcedureQuery(storeInfo, queryParam, cnn, tran: tran);

        //    //Thực hiện
        //    using (var reader = await query.QueryCommand.ExecuteReaderAsync())
        //    {
        //        //output param
        //        if (query.OutputParams.Count > 0
        //            && await reader.NextResultAsync()
        //            && await reader.ReadAsync())
        //        {
        //            foreach (var item in query.OutputParams)
        //            {
        //                queryParam.Set(item, reader.GetValue(item));
        //            }
        //        }

        //        return reader.RecordsAffected;
        //    }
        //}

        ///// <summary>
        ///// Thực thi thủ tục trả về giá trị cell đầu tiên
        ///// </summary>
        //private async Task<T> ExecuteProcedureScalarAsync<T>(string storeName, IStoreQueryParam queryParam, IDbConnection cnn, IDbTransaction tran = null)
        //{
        //    //Kiểm tra sqlinjection
        //    this.ValidateStore(storeName);

        //    //lấy thông tin thủ tục
        //    var storeInfo = this.GetProcedureInfo(cnn, storeName);

        //    //build câu lệnh gọi db
        //    var query = this.BuildProcedureQuery(storeInfo, queryParam, cnn, tran: tran);
        //    T result = default(T);

        //    //Thực hiện
        //    using (var reader = await query.QueryCommand.ExecuteReaderAsync())
        //    {
        //        //data
        //        if (await reader.ReadAsync())
        //        {
        //            result = await reader.GetFieldValueAsync<T>(0);
        //        }

        //        //output param
        //        if (query.OutputParams.Count > 0
        //            && await reader.NextResultAsync()
        //            && await reader.ReadAsync())
        //        {
        //            foreach (var item in query.OutputParams)
        //            {
        //                queryParam.Set(item, reader.GetValue(item));
        //            }
        //        }
        //    }

        //    return result;
        //}

        ///// <summary>
        ///// Thực thi thủ tục trả về giá trị cell đầu tiên
        ///// </summary>
        //private async Task<object> ExecuteProcedureScalarAsync(string storeName, IStoreQueryParam queryParam, IDbConnection cnn, IDbTransaction tran = null)
        //{
        //    //Kiểm tra sqlinjection
        //    this.ValidateStore(storeName);

        //    //lấy thông tin thủ tục
        //    var storeInfo = this.GetProcedureInfo(cnn, storeName);

        //    //build câu lệnh gọi db
        //    var query = this.BuildProcedureQuery(storeInfo, queryParam, cnn, tran: tran);
        //    object result = null;

        //    //Thực hiện
        //    using (var reader = await query.QueryCommand.ExecuteReaderAsync())
        //    {
        //        //data
        //        if (await reader.ReadAsync())
        //        {
        //            result = reader.GetValue(0);
        //        }

        //        //output param
        //        if (query.OutputParams.Count > 0
        //            && await reader.NextResultAsync()
        //            && await reader.ReadAsync())
        //        {
        //            foreach (var item in query.OutputParams)
        //            {
        //                queryParam.Set(item, reader.GetValue(item));
        //            }
        //        }
        //    }

        //    return result;
        //}

        ///// <summary>
        ///// Thực thi thủ tục trả về mảng dữ liệu
        ///// </summary>
        //private async Task<List<T>> ExecuteProcedureReaderAsync<T>(string storeName, IStoreQueryParam queryParam, IDbConnection cnn, IDbTransaction tran = null)
        //{
        //    //Kiểm tra sqlinjection
        //    this.ValidateStore(storeName);

        //    //lấy thông tin thủ tục
        //    var storeInfo = this.GetProcedureInfo(cnn, storeName);

        //    //build câu lệnh gọi db
        //    var query = this.BuildProcedureQuery(storeInfo, queryParam, cnn, tran: tran);

        //    //Thực hiện
        //    var result = new List<T>();
        //    IMapDataReader mapper = null;

        //    using (var reader = await query.QueryCommand.ExecuteReaderAsync())
        //    {
        //        //data
        //        while (await reader.ReadAsync())
        //        {
        //            //check map
        //            if (mapper == null)
        //            {
        //                mapper = this.GetMapper<T>(reader);
        //            }

        //            var obj = (T)mapper.Map(reader);
        //            result.Add(obj);
        //        }

        //        //output param
        //        if (query.OutputParams.Count > 0
        //            && await reader.NextResultAsync()
        //            && await reader.ReadAsync())
        //        {
        //            foreach (var item in query.OutputParams)
        //            {
        //                queryParam.Set(item, reader.GetValue(item));
        //            }
        //        }
        //    }

        //    return result;
        //}

        ///// <summary>
        ///// Tùy thuộc vào kiểu dữ liệu trả về sẽ tạo đối tượng đọc dữ liệu từ reader ra tương ứng
        ///// </summary>
        //private IMapDataReader GetMapper<T>(DbDataReader reader)
        //{
        //    var type = typeof(T);
        //    if (!"System.Object".Equals(type.FullName, StringComparison.OrdinalIgnoreCase)
        //        && type.FullName != typeof(Dictionary<string, object>).FullName)
        //    {
        //        return new ClassMapDataReader<T>(reader);
        //    }

        //    return new DictionaryMapDataReader();
        //}

        ///// <summary>
        ///// Build lại tham số dựa vào entity truyền vào và tham số trong store
        ///// </summary>
        //private ProcedureQuery BuildProcedureQuery(StoreProcedureInfo storeInfo, IStoreQueryParam queryParam, IDbConnection cnn, IDbTransaction tran = null)
        //{
        //    var result = new ProcedureQuery()
        //    {
        //        QueryCommand = new MssqlCommand()
        //        {
        //            Connection = (SqlConnection)cnn,
        //            Transaction = (MssqlTransaction)tran,
        //        },
        //        OutputParams = new List<string>()
        //    };

        //    var storeParamCount = storeInfo.Parameters.Count;
        //    if (storeParamCount > 0
        //        && (queryParam.TotalField < storeParamCount))
        //    {
        //        throw new Exception($"Mismatch param, storeInfo: {JsonConvert.SerializeObject(storeInfo)}, paramArray: {JsonConvert.SerializeObject(queryParam.Get())}");
        //    }

        //    var cmd = result.QueryCommand;
        //    var sbPrefix = new StringBuilder();
        //    var sbSubfix = new StringBuilder();
        //    var sbQuery = new StringBuilder();
        //    for (var i = 0; i < storeParamCount; i++)
        //    {
        //        var p = storeInfo.Parameters[i];
        //        if (string.IsNullOrWhiteSpace(p.Name))
        //        {
        //            throw new Exception($"Mismatch param, storeInfo: {JsonConvert.SerializeObject(storeInfo)}");
        //        }
        //        var name = p.Name.Replace(p.Name.Contains("$") ? "@$" : "@", "");
        //        var value = queryParam.Get(name, i);
        //        var queryParamName = p.Name;
        //        string outputParam = null;
        //        switch (p.Direction)
        //        {
        //            case ParameterDirection.Input:
        //                cmd.Parameters.Add(new MssqlParameter
        //                {
        //                    ParameterName = p.Name,
        //                    DbType = p.DbType,
        //                    Value = value,
        //                    Direction = ParameterDirection.Input,
        //                });
        //                break;

        //            case ParameterDirection.InputOutput:
        //                //Tạo 1 tham số để gọi, tham số gốc sẽ truyền vào dạng input
        //                queryParamName = $"{p.Name}_i";

        //                cmd.Parameters.Add(new MssqlParameter
        //                {
        //                    ParameterName = p.Name,
        //                    Value = value,
        //                    DbType = p.DbType,
        //                    Direction = ParameterDirection.Input,
        //                });

        //                cmd.Parameters.Add(new MssqlParameter
        //                {
        //                    ParameterName = queryParamName,
        //                    DbType = p.DbType,
        //                    Direction = ParameterDirection.ReturnValue,
        //                });

        //                sbPrefix.Append($"SET {queryParamName}={p.Name};");

        //                outputParam = name;
        //                break;
        //            case ParameterDirection.Output:
        //            case ParameterDirection.ReturnValue:
        //                //tạo luôn tham số returnValue
        //                cmd.Parameters.Add(new MssqlParameter
        //                {
        //                    ParameterName = p.Name,
        //                    DbType = p.DbType,
        //                    Direction = ParameterDirection.ReturnValue,
        //                });

        //                outputParam = name;
        //                break;
        //        }

        //        //ghép vào câu lệnh gọi thủ tục
        //        if (sbQuery.Length > 0)
        //        {
        //            sbQuery.Append(",");
        //        }
        //        sbQuery.Append($"{queryParamName}");

        //        //xử lý output
        //        if (outputParam != null)
        //        {
        //            if (sbSubfix.Length == 0)
        //            {
        //                sbSubfix.Append("SELECT ");
        //            }
        //            else
        //            {
        //                sbSubfix.Append(", ");
        //            }
        //            sbSubfix.Append($"{queryParamName} as {outputParam}");
        //            result.OutputParams.Add(outputParam);
        //        }
        //    }

        //    result.QueryCommand.CommandText = $"{sbPrefix}CALL {storeInfo.Name}({sbQuery});{sbSubfix};";
        //    return result;
        //}

        ///// <summary>
        ///// Lấy danh sách tham số của thủ tục
        ///// </summary>
        //protected virtual StoreProcedureInfo GetProcedureInfo(IDbConnection cnn, string storeName, IDbTransaction transaction = null)
        //{
        //    StoreProcedureInfo result = null;
        //    var cacheService = _serviceProvider.GetService<IMemoryCache>();
        //    string cacheKey = $"ProcInfo_{storeName}";
        //    if (cacheService != null)
        //    {
        //        result = cacheService.Get<StoreProcedureInfo>(cacheKey);
        //    }

        //    if (result == null)
        //    {
        //        result = this.GetProcedureInfoDb(cnn, storeName, transaction);
        //        if (result != null && cacheService != null)
        //        {
        //            cacheService.Set(cacheKey, result, DateTimeOffset.Now.AddHours(4));
        //        }
        //    }

        //    if (result == null)
        //    {
        //        throw new Exception($"Notfound procedure {storeName}");
        //    }

        //    return result;
        //}

        ///// <summary>
        ///// Deriver db để lấy thông tin tham số của thủ tục
        ///// </summary>
        //private StoreProcedureInfo GetProcedureInfoDb(IDbConnection cnn, string storeName, IDbTransaction transaction = null)
        //{
        //    var result = new StoreProcedureInfo()
        //    {
        //        Name = storeName,
        //        Parameters = new List<StoreProcedureParam>()
        //    };

        //    using (var cmd = cnn.CreateCommand())
        //    {
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.CommandText = storeName;
        //        cmd.Transaction = transaction;
        //        MssqlCommandBuilder.DeriveParameters((MssqlCommand)cmd);

        //        foreach (MssqlParameter item in cmd.Parameters)
        //        {
        //            result.Parameters.Add(new StoreProcedureParam
        //            {
        //                Name = item.ParameterName,
        //                Direction = item.Direction,
        //                DbType = item.DbType
        //            });
        //        }
        //    }

        //    return result;
        //}

        /// <summary>
        /// Lấy kết nối
        /// </summary>
        public IDbConnection GetConnection()
        {
            var cnn = new SqlConnection(_connectionString);
            return cnn;
        }

        /// <summary>
        /// Lấy SQL connection
        /// </summary>
        public IDbConnection GetConnectionAsync()
        {
            var cnn = GetConnection();
            cnn.Open();
            return cnn;
        }

        /// <summary>
        /// Lấy SQL connection
        /// </summary>
        public async Task<IDbConnection> OpenConnectionAsync()
        {
            var cnn = (SqlConnection)GetConnection();
            await cnn.OpenAsync();
            return cnn;
        }

        public string GetConnectionString()
        {
            return _connectionString;
        }

        public virtual async Task CloseConnectionAsync(IDbConnection connection)
        {
            if (connection != null)
            {
                await ((SqlConnection)connection).CloseAsync();
                connection.Dispose();
            }
        }

        /// <summary>
        /// Xử lý câu lệnh trước khi thực thi
        /// </summary>
        /// <param name="sql">Câu lệnh truy vấn</param>
        public string ProcessSqlBeforExecute(string sql)
        {
            var result = sql;
            //remove words
            if (_InjectionRemoveWords != null)
            {
                foreach (var item in _InjectionRemoveWords)
                {
                    result = Regex.Replace(result, item.Key, item.Value);
                }
            }

            //kiểm tra injection
            this.ValidateSqlInjection(result);

            return result;
        }

        /// <summary>
        /// Kiểm tra cấu lệnh sql có dính injection không
        /// </summary>
        /// <param name="sql">câu lệnh</param>
        private void ValidateSqlInjection(string sql)
        {
            var checkSql = sql;
            //remove ignore words
            if (_InjectionIgnoreWords != null)
            {
                foreach (var item in _InjectionIgnoreWords)
                {
                    checkSql = Regex.Replace(checkSql, item, _InjectionIgnoreWordsReplace, RegexOptions.IgnoreCase);
                }
            }

            //check black words
            foreach (var item in _InjectionWords)
            {
                if (checkSql.IndexOf(item, StringComparison.OrdinalIgnoreCase) > -1)
                {
                    throw new Exception($"Query invalid {item} : {checkSql}");
                }
            }
        }

        #endregion

        /// <summary>
        /// Tạo danh sách theo type
        /// </summary>
        /// <param name="listType">Kiểu dữ liệu của List</param>
        private IList CreateList(Type type)
        {
            Type listType = typeof(List<>).MakeGenericType(new[] { type });
            IList list = (IList)Activator.CreateInstance(listType);
            return list;
        }

        ///// <summary>
        ///// Kiểm tra tên thủ tục trước khi thực thi
        ///// </summary>
        ///// <param name="storeName">tên thủ tục</param>
        //private void ValidateStore(string storeName)
        //{
        //    //kiểm tra injection
        //    this.ValidateSqlInjection(storeName);
        //}
    }

    //public class StoreProcedureInfo
    //{
    //    public string Name { get; set; }
    //    public List<StoreProcedureParam> Parameters { get; set; }
    //}

    //public class StoreProcedureParam
    //{
    //    public string Name { get; set; }
    //    public ParameterDirection Direction { get; set; }
    //    public DbType DbType { get; set; }
    //}
}
