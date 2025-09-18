using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xim.Domain.Mssql.Providers
{
    public interface IMssqlDatabaseProvider
    {
        /// <summary>
        /// Lấy chuỗi kết nối dữ liệu
        /// </summary>
        string GetConnectionString();
        /// <summary>
        /// Lấy kết nối và mở luôn
        /// </summary>
        Task<IDbConnection> OpenConnectionAsync();
        /// <summary>
        /// Giải phóng connection
        /// </summary>
        /// <param name="connection">SQL connection</param>
        Task CloseConnectionAsync(IDbConnection connection);

        //#region Store

        //#region Read data

        ///// <summary>
        ///// Thực hiện thủ tục trả về danh sách dữ liệu
        ///// </summary>
        ///// <typeparam name="T">Kiểu dữ liệu trả về</typeparam>
        ///// <param name="storeName">Tên thủ tục</param>
        ///// <param name="param">Tham số là object</param>
        //Task<List<T>> ExecuteStoreQueryAsync<T>(string storeName, object param = null);

        ///// <summary>
        ///// Thực hiện thủ tục trả về danh sách dữ liệu
        ///// </summary>
        ///// <typeparam name="T">Kiểu dữ liệu trả về</typeparam>
        ///// <param name="storeName">Tên thủ tục</param>
        ///// <param name="param">Tham số là dictionary</param>
        //Task<List<T>> ExecuteStoreQueryAsync<T>(string storeName, Dictionary<string, object> param = null);
        ///// <summary>
        ///// Thực hiện thủ tục trả về danh sách dữ liệu
        ///// </summary>
        ///// <typeparam name="T">Kiểu dữ liệu trả về</typeparam>
        ///// <param name="storeName">Tên thủ tục</param>
        ///// <param name="param">Tham số là mảng object</param>
        //Task<List<T>> ExecuteStoreQueryAsync<T>(string storeName, object[] param = null);

        ///// <summary>
        ///// Thực hiện thủ tục trả về danh sách dữ liệu
        ///// </summary>
        ///// <typeparam name="T">Kiểu dữ liệu trả về</typeparam>
        ///// <param name="cnn">Kết nối tới database</param>
        ///// <param name="storeName">Tên thủ tục</param>
        ///// <param name="param">Tham số là object</param>
        //Task<List<T>> ExecuteStoreQueryAsync<T>(IDbConnection cnn, string storeName, object param = null);

        ///// <summary>
        ///// Thực hiện thủ tục trả về danh sách dữ liệu
        ///// </summary>
        ///// <typeparam name="T">Kiểu dữ liệu trả về</typeparam>
        ///// <param name="cnn">Kết nối tới database</param>
        ///// <param name="storeName">Tên thủ tục</param>
        ///// <param name="param">Tham số là mảng dictionary</param>
        //Task<List<T>> ExecuteStoreQueryAsync<T>(IDbConnection cnn, string storeName, Dictionary<string, object> param = null);

        ///// <summary>
        ///// Thực hiện thủ tục trả về danh sách dữ liệu
        ///// </summary>
        ///// <typeparam name="T">Kiểu dữ liệu trả về</typeparam>
        ///// <param name="cnn">Kết nối tới database</param>
        ///// <param name="storeName">Tên thủ tục</param>
        ///// <param name="param">Tham số là mảng object</param>
        //Task<List<T>> ExecuteStoreQueryAsync<T>(IDbConnection cnn, string storeName, object[] param = null);

        ///// <summary>
        ///// Thực hiện thủ tục trả về danh sách dữ liệu
        ///// </summary>
        ///// <typeparam name="T">Kiểu dữ liệu trả về</typeparam>
        ///// <param name="tran">transaction đang thực thi</param>
        ///// <param name="storeName">Tên thủ tục</param>
        ///// <param name="param">Tham số là object</param>
        //Task<List<T>> ExecuteStoreQueryAsync<T>(IDbTransaction tran, string storeName, object param = null);

        ///// <summary>
        ///// Thực hiện thủ tục trả về danh sách dữ liệu
        ///// </summary>
        ///// <typeparam name="T">Kiểu dữ liệu trả về</typeparam>
        ///// <param name="tran">transaction đang thực thi</param>
        ///// <param name="storeName">Tên thủ tục</param>
        ///// <param name="param">Tham số là mảng dictionary</param>
        //Task<List<T>> ExecuteStoreQueryAsync<T>(IDbTransaction tran, string storeName, Dictionary<string, object> param = null);

        ///// <summary>
        ///// Thực hiện thủ tục trả về danh sách dữ liệu
        ///// </summary>
        ///// <typeparam name="T">Kiểu dữ liệu trả về</typeparam>
        ///// <param name="tran">transaction đang thực thi</param>
        ///// <param name="storeName">Tên thủ tục</param>
        ///// <param name="param">Tham số là mảng object</param>
        //Task<List<T>> ExecuteStoreQueryAsync<T>(IDbTransaction tran, string storeName, object[] param = null);
        //#endregion

        //#region NonQuery

        ///// <summary>
        ///// Thực hiện thủ tục trả về số lượng bản ghi effect
        ///// </summary>
        ///// <param name="storeName">Tên thủ tục</param>
        ///// <param name="param">Tham số là object</param>
        //Task<int> ExecuteStoreNonQueryAsync(string storeName, object param = null);

        ///// <summary>
        ///// Thực hiện thủ tục trả về số lượng bản ghi effect
        ///// </summary>
        ///// <param name="storeName">Tên thủ tục</param>
        ///// <param name="param">Tham số là dictionary</param>
        //Task<int> ExecuteStoreNonQueryAsync(string storeName, Dictionary<string, object> param = null);

        ///// <summary>
        ///// Thực hiện thủ tục trả về số lượng bản ghi effect
        ///// </summary>
        ///// <param name="storeName">Tên thủ tục</param>
        ///// <param name="param">Tham số là mảng object</param>
        //Task<int> ExecuteStoreNonQueryAsync(string storeName, object[] param = null);

        ///// <summary>
        ///// Thực hiện thủ tục trả về số lượng bản ghi effect
        ///// </summary>
        ///// <param name="cnn">Kết nối tới database</param>
        ///// <param name="storeName">Tên thủ tục</param>
        ///// <param name="param">Tham số là object</param>
        //Task<int> ExecuteStoreNonQueryAsync(IDbConnection cnn, string storeName, object param = null);

        ///// <summary>
        ///// Thực hiện thủ tục trả về số lượng bản ghi effect
        ///// </summary>
        ///// <param name="cnn">Kết nối tới database</param>
        ///// <param name="storeName">Tên thủ tục</param>
        ///// <param name="param">Tham số là dictionary</param>
        //Task<int> ExecuteStoreNonQueryAsync(IDbConnection cnn, string storeName, Dictionary<string, object> param = null);

        ///// <summary>
        ///// Thực hiện thủ tục trả về số lượng bản ghi effect
        ///// </summary>
        ///// <param name="cnn">Kết nối tới database</param>
        ///// <param name="storeName">Tên thủ tục</param>
        ///// <param name="param">Tham số là mảng object</param>
        //Task<int> ExecuteStoreNonQueryAsync(IDbConnection cnn, string storeName, object[] param = null);

        ///// <summary>
        ///// Thực hiện thủ tục trả về số lượng bản ghi effect
        ///// </summary>
        ///// <param name="tran">transaction đang thực thi</param>
        ///// <param name="storeName">Tên thủ tục</param>
        ///// <param name="param">Tham số là object</param>
        //Task<int> ExecuteStoreNonQueryAsync(IDbTransaction tran, string storeName, object param = null);

        ///// <summary>
        ///// Thực hiện thủ tục trả về số lượng bản ghi effect
        ///// </summary>
        ///// <param name="tran">transaction đang thực thi</param>
        ///// <param name="storeName">Tên thủ tục</param>
        ///// <param name="param">Tham số là dictionary</param>
        //Task<int> ExecuteStoreNonQueryAsync(IDbTransaction tran, string storeName, Dictionary<string, object> param = null);

        ///// <summary>
        ///// Thực hiện thủ tục trả về số lượng bản ghi effect
        ///// </summary>
        ///// <param name="tran">transaction đang thực thi</param>
        ///// <param name="storeName">Tên thủ tục</param>
        ///// <param name="param">Tham số là mảng object</param>
        //Task<int> ExecuteStoreNonQueryAsync(IDbTransaction tran, string storeName, object[] param = null);

        //#endregion

        //#region Scala
        ///// <summary>
        ///// Thực hiện thủ tục trả về giá trị
        ///// </summary>
        ///// <typeparam name="T">Kiểu dữ liệu trả về</typeparam>
        ///// <param name="storeName">Tên thủ tục</param>
        ///// <param name="param">Tham số là object</param>
        //Task<T> ExecuteStoreScalarAsync<T>(string storeName, object param = null);

        ///// <summary>
        ///// Thực hiện thủ tục trả về giá trị
        ///// </summary>
        ///// <typeparam name="T">Kiểu dữ liệu trả về</typeparam>
        ///// <param name="storeName">Tên thủ tục</param>
        ///// <param name="param">Tham số là dictionary</param>
        //Task<T> ExecuteStoreScalarAsync<T>(string storeName, Dictionary<string, object> param = null);

        ///// <summary>
        ///// Thực hiện thủ tục trả về giá trị
        ///// </summary>
        ///// <typeparam name="T">Kiểu dữ liệu trả về</typeparam>
        ///// <param name="storeName">Tên thủ tục</param>
        ///// <param name="param">Tham số là mảng object</param>
        //Task<T> ExecuteStoreScalarAsync<T>(string storeName, object[] param = null);

        ///// <summary>
        ///// Thực hiện thủ tục trả về giá trị
        ///// </summary>
        ///// <typeparam name="T">Kiểu dữ liệu trả về</typeparam>
        ///// <param name="cnn">Kết nối tới database</param>
        ///// <param name="storeName">Tên thủ tục</param>
        ///// <param name="param">Tham số là object</param>
        //Task<T> ExecuteStoreScalarAsync<T>(IDbConnection cnn, string storeName, object param = null);

        ///// <summary>
        ///// Thực hiện thủ tục trả về giá trị
        ///// </summary>
        ///// <typeparam name="T">Kiểu dữ liệu trả về</typeparam>
        ///// <param name="cnn">Kết nối tới database</param>
        ///// <param name="storeName">Tên thủ tục</param>
        ///// <param name="param">Tham số là dictionary</param>
        //Task<T> ExecuteStoreScalarAsync<T>(IDbConnection cnn, string storeName, Dictionary<string, object> param = null);

        ///// <summary>
        ///// Thực hiện thủ tục trả về giá trị
        ///// </summary>
        ///// <typeparam name="T">Kiểu dữ liệu trả về</typeparam>
        ///// <param name="cnn">Kết nối tới database</param>
        ///// <param name="storeName">Tên thủ tục</param>
        ///// <param name="param">Tham số là mảng object</param>
        //Task<T> ExecuteStoreScalarAsync<T>(IDbConnection cnn, string storeName, object[] param = null);

        ///// <summary>
        ///// Thực hiện thủ tục trả về giá trị
        ///// </summary>
        ///// <typeparam name="T">Kiểu dữ liệu trả về</typeparam>
        ///// <param name="tran">transaction đang thực thi</param>
        ///// <param name="storeName">Tên thủ tục</param>
        ///// <param name="param">Tham số là object</param>
        //Task<T> ExecuteStoreScalarAsync<T>(IDbTransaction tran, string storeName, object param = null);

        ///// <summary>
        ///// Thực hiện thủ tục trả về giá trị
        ///// </summary>
        ///// <typeparam name="T">Kiểu dữ liệu trả về</typeparam>
        ///// <param name="tran">transaction đang thực thi</param>
        ///// <param name="storeName">Tên thủ tục</param>
        ///// <param name="param">Tham số là dictionary</param>
        //Task<T> ExecuteStoreScalarAsync<T>(IDbTransaction tran, string storeName, Dictionary<string, object> param = null);

        ///// <summary>
        ///// Thực hiện thủ tục trả về giá trị
        ///// </summary>
        ///// <typeparam name="T">Kiểu dữ liệu trả về</typeparam>
        ///// <param name="tran">transaction đang thực thi</param>
        ///// <param name="storeName">Tên thủ tục</param>
        ///// <param name="param">Tham số là mảng object</param>
        //Task<T> ExecuteStoreScalarAsync<T>(IDbTransaction tran, string storeName, object[] param = null);

        ////--
        ///// <summary>
        ///// Thực hiện thủ tục trả về giá trị
        ///// </summary>
        ///// <param name="storeName">Tên thủ tục</param>
        ///// <param name="param">Tham số là object</param>
        ///// <returns>Trả về object, khi sử dụng tự ép kiểu</returns>
        //Task<object> ExecuteStoreScalarAsync(string storeName, object param = null);

        ///// <summary>
        ///// Thực hiện thủ tục trả về giá trị
        ///// </summary>
        ///// <param name="storeName">Tên thủ tục</param>
        ///// <param name="param">Tham số là dictionary</param>
        ///// <returns>Trả về object, khi sử dụng tự ép kiểu</returns>
        //Task<object> ExecuteStoreScalarAsync(string storeName, Dictionary<string, object> param = null);

        ///// <summary>
        ///// Thực hiện thủ tục trả về giá trị
        ///// </summary>
        ///// <param name="storeName">Tên thủ tục</param>
        ///// <param name="param">Tham số là mảng object</param>
        ///// <returns>Trả về object, khi sử dụng tự ép kiểu</returns>
        //Task<object> ExecuteStoreScalarAsync(string storeName, object[] param = null);

        ///// <summary>
        ///// Thực hiện thủ tục trả về giá trị
        ///// </summary>
        ///// <param name="cnn">Kết nối tới database</param>
        ///// <param name="storeName">Tên thủ tục</param>
        ///// <param name="param">Tham số là object</param>
        ///// <returns>Trả về object, khi sử dụng tự ép kiểu</returns>
        //Task<object> ExecuteStoreScalarAsync(IDbConnection cnn, string storeName, object param = null);

        ///// <summary>
        ///// Thực hiện thủ tục trả về giá trị
        ///// </summary>
        ///// <param name="cnn">Kết nối tới database</param>
        ///// <param name="storeName">Tên thủ tục</param>
        ///// <param name="param">Tham số là dictionary</param>
        ///// <returns>Trả về object, khi sử dụng tự ép kiểu</returns>
        //Task<object> ExecuteStoreScalarAsync(IDbConnection cnn, string storeName, Dictionary<string, object> param = null);

        ///// <summary>
        ///// Thực hiện thủ tục trả về giá trị
        ///// </summary>
        ///// <param name="cnn">Kết nối tới database</param>
        ///// <param name="storeName">Tên thủ tục</param>
        ///// <param name="param">Tham số là mảng object</param>
        ///// <returns>Trả về object, khi sử dụng tự ép kiểu</returns>
        //Task<object> ExecuteStoreScalarAsync(IDbConnection cnn, string storeName, object[] param = null);

        ///// <summary>
        ///// Thực hiện thủ tục trả về giá trị
        ///// </summary>
        ///// <param name="tran">transaction đang thực thi</param>
        ///// <param name="storeName">Tên thủ tục</param>
        ///// <param name="param">Tham số là object</param>
        ///// <returns>Trả về object, khi sử dụng tự ép kiểu</returns>
        //Task<object> ExecuteStoreScalarAsync(IDbTransaction tran, string storeName, object param = null);

        ///// <summary>
        ///// Thực hiện thủ tục trả về giá trị
        ///// </summary>
        ///// <param name="tran">transaction đang thực thi</param>
        ///// <param name="storeName">Tên thủ tục</param>
        ///// <param name="param">Tham số là dictionary</param>
        ///// <returns>Trả về object, khi sử dụng tự ép kiểu</returns>
        //Task<object> ExecuteStoreScalarAsync(IDbTransaction tran, string storeName, Dictionary<string, object> param = null);

        ///// <summary>
        ///// Thực hiện thủ tục trả về giá trị
        ///// </summary>
        ///// <param name="tran">transaction đang thực thi</param>
        ///// <param name="storeName">Tên thủ tục</param>
        ///// <param name="param">Tham số là mảng object</param>
        ///// <returns>Trả về object, khi sử dụng tự ép kiểu</returns>
        //Task<object> ExecuteStoreScalarAsync(IDbTransaction tran, string storeName, object[] param = null);
        //#endregion

        //#endregion
        /// <summary>
        /// Thực hiện sql trả về row effect
        /// </summary>
        /// <param name="commandText">câu truy vấn</param>
        /// <param name="param">Tham số là dictionary</param>
        /// <param name="timeout">Thời gian xử lý tối đa</param>
        Task<int> ExecuteNonQueryTextAsync(string commandText, Dictionary<string, object> param, int? timeout = null);
        /// <summary>
        /// Thực hiện sql trả về row effect
        /// </summary>
        /// <param name="commandText">câu truy vấn</param>
        /// <param name="param">Tham số là dictionary</param>
        Task<int> ExecuteNonQueryTextAsync(IDbConnection cnn, string commandText, Dictionary<string, object> param, int? timeout = null);
        /// <summary>
        /// Thực hiện sql trả về row effect
        /// </summary>
        /// <param name="transaction">transaction</param>
        /// <param name="commandText">câu truy vấn</param>
        /// <param name="param">Tham số là dictionary</param>
        Task<int> ExecuteNonQueryTextAsync(IDbTransaction transaction, string commandText, Dictionary<string, object> param);
        /// <summary>
        /// Thực hiện sql trả về row effect
        /// </summary>
        /// <param name="commandText">câu truy vấn</param>
        /// <param name="param">Tham số là dictionary</param>
        Task<int> ExecuteNonQueryTextAsync(string commandText, object param);
        /// <summary>
        /// Thực hiện sql trả về row effect
        /// </summary>
        /// <param name="commandText">câu truy vấn</param>
        /// <param name="param">Tham số là dictionary</param>
        Task<int> ExecuteNonQueryTextAsync(IDbConnection cnn, string commandText, object param);
        /// <summary>
        /// Thực hiện sql trả về row effect
        /// </summary>
        /// <param name="transaction">transaction</param>
        /// <param name="commandText">câu truy vấn</param>
        /// <param name="param">Tham số là dictionary</param>
        Task<int> ExecuteNonQueryTextAsync(IDbTransaction transaction, string commandText, object param);
        /// <summary>
        /// Thực hiện sql trả về dữ liệu cell đầu tiên
        /// </summary>
        /// <param name="commandText">câu truy vấn</param>
        /// <param name="param">Tham số là dictionary</param>
        Task<object> ExecuteScalarTextAsync(string commandText, Dictionary<string, object> param);
        /// <summary>
        /// Thực hiện sql trả về dữ liệu cell đầu tiên
        /// </summary>
        /// <param name="commandText">câu truy vấn</param>
        /// <param name="param">Tham số là dictionary</param>
        Task<object> ExecuteScalarTextAsync(IDbConnection cnn, string commandText, Dictionary<string, object> param);
        /// <summary>
        /// Thực hiện sql trả về dữ liệu cell đầu tiên
        /// Dùng cho câu lệnh insert trả về khóa chính bản ghi
        /// </summary>
        /// <param name="commandText">câu truy vấn</param>
        /// <param name="param">Tham số là dictionary</param>
        Task<object> ExecuteScalarTextAsync(IDbTransaction transaction, string commandText, Dictionary<string, object> param);
        /// <summary>
        /// Thực hiện sql trả về dữ liệu cell đầu tiên
        /// </summary>
        /// <param name="commandText">câu truy vấn</param>
        /// <param name="param">Tham số là object</param>
        Task<object> ExecuteScalarTextAsync(string commandText, object param);
        /// <summary>
        /// Thực hiện sql trả về dữ liệu cell đầu tiên
        /// </summary>
        /// <param name="commandText">câu truy vấn</param>
        /// <param name="param">Tham số là object</param>
        Task<object> ExecuteScalarTextAsync(IDbConnection cnn, string commandText, object param);
        /// <summary>
        /// Thực hiện sql trả về dữ liệu cell đầu tiên
        /// Dùng cho câu lệnh insert trả về khóa chính bản ghi
        /// </summary>
        /// <param name="commandText">câu truy vấn</param>
        /// <param name="param">Tham số là object</param>
        Task<object> ExecuteScalarTextAsync(IDbTransaction transaction, string commandText, object param);
        /// <summary>
        /// Thực hiện sql trả về danh sách dữ liệu 
        /// </summary>
        /// <typeparam name="T">Kiểu dữ liệu trả về</typeparam>
        /// <param name="commandText">câu truy vấn</param>
        /// <param name="param">Tham số là dictionary</param>
        Task<List<T>> QueryAsync<T>(string commandText, Dictionary<string, object> param);

        /// <summary>
        /// Thực hiện sql trả về danh sách dữ liệu 
        /// </summary>
        /// <typeparam name="T">Kiểu dữ liệu trả về</typeparam>
        /// <param name="commandText">câu truy vấn</param>
        /// <param name="param">Tham số là dictionary</param>
        Task<List<T>> QueryAsync<T>(IDbConnection cnn, string commandText, Dictionary<string, object> param);

        /// <summary>
        /// Thực hiện sql trả về danh sách dữ liệu 
        /// </summary>
        /// <typeparam name="T">Kiểu dữ liệu trả về</typeparam>
        /// <param name="commandText">câu truy vấn</param>
        /// <param name="param">Tham số là dictionary</param>
        Task<List<T>> QueryAsync<T>(IDbTransaction transaction, string commandText, Dictionary<string, object> param);
        /// <summary>
        /// Thực hiện sql trả về danh sách dữ liệu 
        /// </summary>
        /// <param name="type">Kiểu dữ liệu trả về</param>
        /// <param name="commandText">câu truy vấn</param>
        /// <param name="param">Tham số là dictionary</param>
        Task<IList> QueryAsync(Type type, string commandText, Dictionary<string, object> param);
        /// <summary>
        /// Thực hiện sql trả về danh sách dữ liệu
        /// pvduy 09/03/2021
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="type">Kiểu dữ liệu trả về</param>
        /// <param name="commandText">câu truy vấn</param>
        /// <param name="param">Tham số là dictionary</param>
        /// <returns></returns>
        Task<IList> QueryAsync(IDbTransaction transaction, Type type, string commandText, Dictionary<string, object> param);
        /// <summary>
        /// Thực hiện sql trả về danh sách dữ liệu 
        /// </summary>
        /// <param name="type">Kiểu dữ liệu trả về</param>
        /// <param name="commandText">câu truy vấn</param>
        /// <param name="param">Tham số là dictionary</param>
        Task<IList> QueryAsync(IDbConnection cnn, Type type, string commandText, Dictionary<string, object> param);
        /// <summary>
        /// Thực hiện sql trả về danh sách dữ liệu 
        /// </summary>
        /// <param name="commandText">câu truy vấn</param>
        /// <param name="param">Tham số là dictionary</param>
        Task<IList> QueryAsync(string commandText, Dictionary<string, object> param);
        /// <summary>
        /// Thực hiện sql trả về danh sách dữ liệu 
        /// </summary>
        /// <param name="commandText">câu truy vấn</param>
        /// <param name="param">Tham số là dictionary</param>
        Task<IList> QueryAsync(IDbConnection cnn, string commandText, Dictionary<string, object> param);
        /// <summary>
        /// Thực hiện sql trả về danh sách dữ liệu với thủ tục hoặc là Text
        /// </summary>
        /// <param name="cnn">Kết nối</param>
        /// <param name="commandText">Nội dung sql</param>
        /// <param name="pCommandType">Loại query: command text, proc</param>
        /// <param name="param">Tham số</param>
        Task<List<T>> QueryWithCommandTypeAsync<T>(IDbConnection cnn, string commandText, CommandType pCommandType, Dictionary<string, object> param);

        /// <summary>
        /// Thực hiện sql trả về danh sách dữ liệu với thủ tục hoặc là Text
        /// </summary>
        /// <typeparam name="T">Kiểu dữ liệu trả về</typeparam>
        /// <param name="transaction">Transaction thực hiện</param>
        /// <param name="commandText">Nội dung sql</param>
        /// <param name="pCommandType">Loại query: command text, proc</param>
        /// <param name="param">Tham số</param>
        Task<List<T>> QueryWithCommandTypeWithTranAsync<T>(IDbTransaction transaction, string commandText, CommandType pCommandType, Dictionary<string, object> param);
    }
}
