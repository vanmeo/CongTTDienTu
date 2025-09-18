using System;
using System.Collections.Generic;
using System.Text;

namespace Xim.Storage.FileStorages
{
    /// <summary>
    /// Cấu hình quản lý file
    /// </summary>
    public class FileStorageConfig
    {
        /// <summary>
        /// Đường dẫn gốc mount vào để chạy
        /// </summary>
        public string BasePath { get; set; }
    }
}
