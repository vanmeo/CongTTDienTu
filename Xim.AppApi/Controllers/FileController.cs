//using ImageMagick;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Routing.Constraints;
//using System.Net.Mime;
//using System.Net.WebSockets;
//using System.Text;
//using Xim.Storage;

//namespace Xim.AppApi.Controllers
//{
//    //[Authorize]
//    public class FileController : BaseController
//    {
//        private readonly IStorageService _storageService;
//        internal const string DOWNLOAD_CONTENT_TYPE = "application/octect-stream";
//        public FileController(IServiceProvider serviceProvider) : base(serviceProvider)
//        {
//            _storageService = serviceProvider.GetService<IStorageService>();
//        }

//        /// <summary>
//        /// force download file
//        /// </summary>
//        [HttpGet("download/{category}/{name}")]
//        [HttpGet("download/{category}/{name}/{new_name}")]
//        [AllowAnonymous]
//        public async Task<IActionResult> DownloadAsync(StorageCategory category, string name, string new_name)
//        {
//            var stream = await _storageService.GetAsync(category, name);
//            string fileName;
//            if (!string.IsNullOrEmpty(new_name))
//            {
//                fileName = $"{Path.GetFileNameWithoutExtension(new_name)}{Path.GetExtension(name)}";
//            }
//            else
//            {
//                fileName = name;
//            }

//            return File(stream, DOWNLOAD_CONTENT_TYPE, fileName);
//        }

//        [AllowAnonymous]
//        [HttpGet("image/{category}/{name}")]
//        public async Task<IActionResult> GetImageAsync(StorageCategory category, string name, int height, int width, int quality)
//        {
//            var cacheName = this.GetImageResizeCacheName(name, height, width, quality);

//            //Có có resize thì kiểm tra cache xem có thì trả về
//            if (cacheName != name)
//            {
//                var cacheMs = await _storageService.GetAsync(category, name: cacheName);
//                if (cacheMs != null && cacheMs.Length > 0)
//                {
//                    return ReturnImage(cacheMs.ToArray(), name);
//                }
//            }

//            var ms = await _storageService.GetAsync(category, name: name);
//            if (ms == null || ms.Length == 0)
//            {
//                return NotFound($"Không tìm thấy file {name} - {category}");
//            }

//            byte[] bytes = null;
//            //resize nếu có truyền kích thước
//            if (height > 0 || width > 0)
//            {
//                try
//                {
//                    await using (var rsMs = await this.ResizeImageAsync(ms, height, width, quality))
//                    {
//                        //nếu resize được thì sẽ cache lại để tái sử dụng
//                        await _storageService.SaveAsync(StorageCategory.Static, cacheName, rsMs);

//                        bytes = rsMs.ToArray();
//                    }
//                }
//                catch (Exception ex)
//                {
//                    var logger = _serviceProvider.GetService<ILogger<FileController>>();
//                    logger.LogError(ex, $"Resize image exception: {ex.Message}");
//                }
//            }

//            if (bytes == null)
//            {
//                bytes = ms.ToArray();
//            }

//            return ReturnImage(bytes, name);
//        }
//        [HttpPost]
//        [Route("UploadImage")]
//        public async Task<IActionResult> UploadImage([FromForm] IFormFile upload, int itype)
//        {
//            if (upload != null && upload.Length > 0)
//            {
//                // Lưu trữ file ảnh vào thư mục Uploads
//                var filePath = Path.Combine("Uploads", upload.FileName);

//                // Tạo thư mục nếu chưa tồn tại
//                Directory.CreateDirectory(Path.GetDirectoryName(filePath));

//                using (var stream = new FileStream(filePath, FileMode.Create))
//                {
//                    await upload.CopyToAsync(stream);
//                }

//                // CKEditor cần phản hồi đặc biệt
//                return Ok(new
//                {
//                    uploaded = true,
//                    url = $"/uploads/{upload.FileName}" // Đường dẫn trả về cho CKEditor để hiển thị ảnh
//                });
//            }

//            return BadRequest("Invalid file upload.");
//        }
//        /// <summary>
//        /// Upload file lên server
//        /// </summary>
//        [HttpPost]
//        [RequestSizeLimit(100000000)]
//        public async Task<IActionResult> UploadAsync([FromForm] StorageCategory category, IFormFile file)
//        {
//            var err = ValidateUpload(category, file);
//            if (!string.IsNullOrEmpty(err))
//            {
//                return StatusCode(StatusCodes.Status500InternalServerError, err);
//            }

//            //var contextService = _serviceProvider.GetRequiredService<IContextService>();
//            //var context = contextService.Get();
//            string fileKey = string.Format(Guid.NewGuid().ToString());
//            string fileExtension = Path.GetExtension(file.FileName);
//            string fileName = $"{fileKey}{fileExtension}".ToLower();

//            using (var stream = file.OpenReadStream())
//            {
//                await _storageService.SaveAsync(StorageCategory.Static, fileName, stream);
//            }

//            return Ok(fileName);
//        }

//        /// <summary>
//        /// Upload file lên server
//        /// </summary>
//        /// <param name="category">Upload file gì: Dùng để validate nghiệp vụ, con sẽ dẩy vào temp</param>
//        /// <param name="files">Danh sách các file</param>
//        /// <returns>Danh sách tên file temp</returns>
//        [RequestSizeLimit(100000000)]
//        [HttpPost("multi")]
//        public async Task<IActionResult> UploadsAsync(StorageCategory category, List<IFormFile> files)
//        {
//            int validCount = 0;
//            int errorCount = 0;
//            var temps = new List<object>();

//            for (var i = 0; i < files.Count; i++)
//            {
//                var item = files[i];
//                var err = ValidateUpload(category, item);
//                if (!string.IsNullOrEmpty(err))
//                {
//                    temps.Add(new { error = err });
//                    errorCount++;
//                }
//                else
//                {
//                    validCount++;
//                    string fileKey = string.Format(Guid.NewGuid().ToString());
//                    string fileExtension = Path.GetExtension(item.FileName);
//                    string fileName = $"{fileKey}{fileExtension}";

//                    using (var stream = item.OpenReadStream())
//                    {
//                        await _storageService.SaveAsync(StorageCategory.Static, fileName, stream);
//                    }

//                    temps.Add(new { name = fileName });
//                }
//            }
//            return Ok(temps);
//        }

//        #region Private

//        /// <summary>
//        /// Trả ảnh về client
//        /// </summary>
//        IActionResult ReturnImage(byte[] content, string name)
//        {
//            var contentType = this.GetContentType(name);
//            var storageConfig = _serviceProvider.GetService<StorageConfig>();
//            if (storageConfig?.ImageCacheSecond > 0)
//            {
//                Response.Headers["Cache-Control"] = $"public,max-age={storageConfig.ImageCacheSecond}";
//                Response.Headers["Expires"] = DateTime.UtcNow.AddSeconds(7).ToString("R");
//            }

//            return new FileContentResult(content, new Microsoft.Net.Http.Headers.MediaTypeHeaderValue(contentType));
//        }

//        /// <summary>
//        /// Lấy tên ảnh lưu cache khi resize
//        /// Mục đích tránh lặp lại công việc nhiều lần tốn tài nguyên
//        /// </summary>
//        string GetImageResizeCacheName(string name, int height, int width, int quality)
//        {
//            var sb = new StringBuilder(Path.GetFileNameWithoutExtension(name));
//            if (height > 0)
//            {
//                sb.Append($"_h_{height}");
//            }
//            else if (width > 0)
//            {
//                sb.Append($"_w_{width}");
//            }
//            else
//            {
//                //Nếu không truyền kích thước trả về tên ảnh gốc
//                return name;
//            }

//            var ql = this.GetImageResizeQuality(quality);
//            sb.Append($"_q_{ql}");
//            sb.Append(Path.GetExtension(name));

//            return sb.ToString();
//        }

//        /// <summary>
//        /// Lấy chất lượng ảnh resize
//        /// </summary>
//        int GetImageResizeQuality(int quality)
//        {
//            if (quality > 50 && quality <= 100)
//            {
//                return quality;
//            }

//            return 80;
//        }

//        /// <summary>
//        /// Tùy chỉnh kích thước của ảnh theo yêu cầu của client
//        /// </summary>
//        async Task<MemoryStream> ResizeImageAsync(MemoryStream ms, int height, int width, int quality)
//        {
//            using (var image = new MagickImage(ms))
//            {
//                if (height > 0)
//                {
//                    width = image.Width * height / image.Height;
//                }
//                else
//                {
//                    height = image.Height * width / image.Width;
//                }

//                image.Resize(width, height);
//                image.Strip();
//                image.Quality = this.GetImageResizeQuality(quality);

//                var cms = new MemoryStream();
//                await image.WriteAsync(cms);
//                cms.Seek(0, SeekOrigin.Begin);

//                return cms;
//            }
//        }

//        /// <summary>
//        /// Kiểm tra việc upload file có hợp lệ không
//        /// </summary>
//        /// <param name="category">file gì</param>
//        /// <param name="file">file</param>
//        string ValidateUpload(StorageCategory category, IFormFile file)
//        {
//            var data = file.FileName.Split(".");

//            //Nếu không có extension -> fail
//            if (data.Count() < 2)
//            {
//                return "File name invalid";
//            }

//            var storageConfig = _serviceProvider.GetRequiredService<StorageConfig>();
//            var ext = data.Last().ToLower();
//            if (storageConfig.UploadAllowExtensions != null
//                && storageConfig.UploadAllowExtensions.Count > 0
//                && !storageConfig.UploadAllowExtensions.Contains(ext))
//            {
//                return "File extension invalid";
//            }

//            if (storageConfig.UploadMaxSizeMB.HasValue
//                && file.Length > storageConfig.UploadMaxSizeMB.Value * 1024 * 1024)
//            {
//                return $"File không được lớn hơn {storageConfig.UploadMaxSizeMB} MB";
//            }

//            return null;
//        }

//        /// <summary>
//        /// Lấy maping content type theo đuôi file
//        /// </summary>
//        string GetContentType(string name)
//        {
//            var extension = Path.GetExtension(name);
//            var config = _serviceProvider.GetRequiredService<StorageConfig>();
//            var map = config.ContentType?.FirstOrDefault(n => n.Key.Equals(extension, StringComparison.OrdinalIgnoreCase));
//            if (map?.Value == null || string.IsNullOrWhiteSpace(map.Value.Value))
//            {
//                return config.ContentTypeDefault;
//            }

//            return map.Value.Value;
//        }

//        #endregion
//    }
//}
