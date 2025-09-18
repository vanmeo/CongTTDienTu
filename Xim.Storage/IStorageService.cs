using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Xim.Storage
{
    public interface IStorageService
    {
        /// <summary>
        /// Lấy file theo loại và tên file
        /// </summary>
        /// <param name="type">Loại file</param>
        /// <param name="name">tên file</param>
        /// <returns>Stream</returns>
        Task<MemoryStream> GetAsync(StorageCategory type, string name = null, string subPath = null);
        /// <summary>
        /// Lấy file theo loại và tên file
        /// </summary>
        /// <param name="type">Loại file</param>
        /// <param name="name">tên file</param>
        /// <returns>String content</returns>
        Task<string> GetStringAsync(StorageCategory type, string name = null, string subPath = null);

        /// <summary>
        /// Lưu file
        /// </summary>
        /// <param name="type">Loại file</param>
        /// <param name="name">Tên file</param>
        /// <param name="content">Nội dung file</param>
        Task SaveAsync(StorageCategory type, string name, Stream content, string subPath = null);

        /// <summary>
        /// Lưu file
        /// </summary>
        /// <param name="type">Loại file</param>
        /// <param name="name">Tên file</param>
        /// <param name="content">Nội dung file</param>
        Task SaveAsync(StorageCategory type, string name, string content, string subPath = null);

        /// <summary>
        /// Xóa file
        /// </summary>
        /// <param name="type">Loại file</param>
        /// <param name="name">Tên file</param>
        Task<bool> DeleteAsync(StorageCategory type, string name, string subPath = null);

        /// <summary>
        /// Copy file từ temp sang thư mục thật
        /// Sau khi copy sẽ xóa file temp đi
        /// </summary>
        /// <param name="fromType">Loại file nguồn</param>
        /// <param name="fromName">Tên file nguồn</param>
        /// <param name="toType">Loại file đích</param>
        /// <param name="toName">Tên file đích, nếu không</param>
        /// <returns>true: thành công, false: thất bại</returns>
        Task<bool> CopyAsync(StorageCategory fromType, string fromName, StorageCategory toType, string toName, string fromSubPath = null, string toSubPath = null);

        /// <summary>
        /// Kiểm tra file có tồn tại không
        /// </summary>
        /// <param name="type">Loại file</param>
        /// <param name="name">tên file</param>
        Task<bool> ExistAsync(StorageCategory type, string name = null, string subPath = null);
    }
}
