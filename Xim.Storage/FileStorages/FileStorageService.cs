using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Xim.Storage.FileStorages
{
    /// <summary>
    /// Quản lý với file
    /// </summary>
    public class FileStorageService : IStorageService
    {
        readonly FileStorageConfig _config;
        public FileStorageService(FileStorageConfig config)
        {
            _config = config;
        }

        void AppendSeperator(StringBuilder sb)
        {
            var last = sb[sb.Length - 1];
            if (last != '/' && last != '\\')
            {
                sb.Append('/');
            }
        }

        string GetPath(StorageCategory type, string name, string subPath)
        {
            var sb = new StringBuilder();
            //base path
            sb.Append(_config.BasePath);

            //vs path
            AppendSeperator(sb);
            switch (type)
            {
                default:
                    sb.Append(type.ToString());
                    break;
            }

            if (!string.IsNullOrEmpty(subPath))
            {
                var sp = subPath.Replace(@"\", "/");
                if (!sp.StartsWith('/'))
                {
                    AppendSeperator(sb);
                }

                if (sp.EndsWith('/'))
                {
                    sp = sp.Substring(0, sp.Length - 1);
                }

                sb.Append(sp);
            }

            //file name
            if (!string.IsNullOrEmpty(name))
            {
                AppendSeperator(sb);
                sb.Append(Path.GetFileName(name));
            }

            return sb.ToString().Trim().ToLower();
        }

        /// <summary>
        /// Tạo thư mục lưu file nếu chưa tồn tại
        /// </summary>
        bool CreateFolderStorageFile(string filePath)
        {
            var folder = Path.GetDirectoryName(filePath);
            if (Directory.Exists(folder))
            {
                return false;
            }

            Directory.CreateDirectory(folder);
            return true;
        }

        /// <summary>
        /// Kiểm tra file tồn tại không
        /// </summary>
        Task<bool> ExistsAsync(string path)
        {
            var rs = File.Exists(path);
            return Task.FromResult(rs);
        }

        public async Task<bool> CopyAsync(StorageCategory fromType, string fromName, StorageCategory toType, string toName, string fromSubPath = null, string toSubPath = null)
        {
            var fromPath = GetPath(fromType, fromName, fromSubPath);
            if (!await ExistsAsync(fromPath))
            {
                return false;
            }

            var toPath = GetPath(toType, toName, toSubPath);
            CreateFolderStorageFile(toPath);
            File.Copy(fromPath, toPath);
            return true;
        }

        public async Task<bool> DeleteAsync(StorageCategory type, string name, string subPath = null)
        {
            var path = GetPath(type, name, subPath);
            if (!await ExistsAsync(path))
            {
                return false;
            }

            File.Delete(path);
            return true;
        }

        public async Task<bool> ExistAsync(StorageCategory type, string name = null, string subPath = null)
        {
            var path = GetPath(type, name, subPath);
            var rs = await ExistsAsync(path);
            return rs;
        }

        public async Task<MemoryStream> GetAsync(StorageCategory type, string name = null, string subPath = null)
        {
            var path = GetPath(type, name, subPath);
            if (!await ExistsAsync(path))
            {
                return null;
            }

            var ms = new MemoryStream();
            await using (var fs = File.OpenRead(path))
            {
                await fs.CopyToAsync(ms);
            }

            ms.Seek(0, SeekOrigin.Begin);
            return ms;
        }

        public async Task<string> GetStringAsync(StorageCategory type, string name = null, string subPath = null)
        {
            var path = GetPath(type, name, subPath);
            if (!await ExistsAsync(path))
            {
                return null;
            }

            var content = await File.ReadAllTextAsync(path);
            return content;
        }

        public async Task SaveAsync(StorageCategory type, string name, Stream content, string subPath = null)
        {
            var path = GetPath(type, name, subPath);
            CreateFolderStorageFile(path);

            await using (var file = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                content.Seek(0, SeekOrigin.Begin);
                await content.CopyToAsync(file);
            }
        }

        public async Task SaveAsync(StorageCategory type, string name, string content, string subPath = null)
        {
            var path = GetPath(type, name, subPath);
            CreateFolderStorageFile(path);

            await File.WriteAllTextAsync(path, content);
        }
    }
}
