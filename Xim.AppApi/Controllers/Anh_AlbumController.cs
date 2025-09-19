using API.Constants;
using FluentFTP;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog.Filters;
using System.Net;
using System.Reflection;
using Xim.Application.Contracts.Album;
using Xim.Application.Contracts.Anh_Album;
using Xim.Application.Contracts.HoatDong;
using Xim.Application.Contracts.NhatKy;
using Xim.Domain.Entities;
using Xim.Domain.Pagings;
using Xim.Library.Extensions;
using System.IO;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Xim.AppApi.Controllers
{
    [Authorize]
    public class Anh_AlbumController : BaseController
    {
        public class AnhUploadDto
        {
            public string Content { get; set; }  // nội dung mô tả
            public IFormFile File { get; set; }  // file ảnh
        }
        private readonly IAnh_AlbumService _service;
        private readonly IWebHostEnvironment _environment;
        /// <summary>
        /// Admin: Phục vụ quản lý Ảnh cập nhật trong 1 album
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="environment"></param>
        public Anh_AlbumController(IServiceProvider serviceProvider, IWebHostEnvironment environment) : base(serviceProvider)
        {
            _service = serviceProvider.GetService<IAnh_AlbumService>();
            _environment = environment;
        }
        /// <summary>
        /// Admin: Liệt kê tất cả  các ảnh cái này chỉ dùng để liệt kê trong form tạo mới ảnh thôi ít dùng
        /// </summary>
        /// <param name="paging">Phân trang gồm page size, page number</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("getall")]

        public async Task<IActionResult> GetListAsync(paging paging)
        {
            int offset = (paging.pageNumber - 1) * paging.pageSize;
            PagingParam param = new PagingParam();
            param.sort = "thutu";
            param.skip = offset;
            param.take = paging.pageSize;
            param.filter = "[{ 'f':'is_deleted','o':'=','v':'0'}]";
            param.columns = "";
            var data = await _service.GetListAsync(param);
            dynamic sumDataDynamic = data.sumData;
            int total = sumDataDynamic.total;
            return Ok(new
            {
                Data = data.data,
                PageSize = paging.pageSize,
                TotalDocuments = total,
                PageNumber = paging.pageNumber,
                TotalPages = (int)Math.Ceiling((double)total / paging.pageSize)
            });
        }
        /// <summary>
        /// ADmin: Thêm mới nhiều file ảnh vào 1 album
        /// </summary>
        /// <param name="contents"> List nội dung của các ảnh </param>
        /// <param name="thumbnail">Danh sách ảnh </param>
        /// <returns>Danh sách file ảnh insert vào bảng</returns>
        [HttpPost("{idalbum}/images")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> InsertMultiImagesAsync(Guid idalbum, [FromForm] List<IFormFile> files, [FromForm] List<string> contents)
        {
            try
            {
                if (files == null || files.Count == 0)
                    return BadRequest("Chưa chọn ảnh nào.");
                if (contents == null || contents.Count != files.Count)
                    return BadRequest("Số lượng nội dung không khớp với số ảnh.");
                using (var ftpClient = new FtpClient
                {
                    Host = GlobalConfig.Ftp.Host,
                    Port = GlobalConfig.Ftp.Port,
                    Credentials = new NetworkCredential(GlobalConfig.Ftp.Username, GlobalConfig.Ftp.Password),
                    DataConnectionType = FtpDataConnectionType.AutoActive
                })
                {
                    // Kết nối
                    ftpClient.Connect();
                    var listAnh = new List<Anh_AlbumDtoCreate>();
                    var contextData = this.GetContext();

                    for (int i = 0; i < files.Count; i++)
                    {
                        var thumbnail = files[i];
                        var imageExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
                        var videoExtensions = new[] { ".mp4", ".avi", ".mkv", ".mov", ".wmv" };

                        var fileExtension = Path.GetExtension(thumbnail.FileName).ToLower();

                        bool isImage = imageExtensions.Contains(fileExtension);
                        bool isVideo = videoExtensions.Contains(fileExtension);

                        if (!isImage && !isVideo)
                            return BadRequest(new { Message = "Invalid file format. Only images or videos are allowed." });

                        // Xác định thư mục theo loại file
                        string subFolder = isImage ? "images" : "videos";
                        string uniqueFileName = $"{Guid.NewGuid()}_{thumbnail.FileName}";
                        // Tạo đường dẫn lưu file trên FTP
                        string remoteDir = Path.Combine(GlobalConfig.Ftp.RemoteDirectory, subFolder).Replace("\\", "/");
                        string remoteFilePath = Path.Combine(remoteDir, uniqueFileName).Replace("\\", "/");


                        // Upload file từ stream
                        using (var stream = thumbnail.OpenReadStream())
                        {
                            ftpClient.Upload(stream, remoteFilePath);
                        }
                        // Ngắt kết nối


                        var anh = new Anh_AlbumDtoCreate
                        {
                            LinkAnh = GlobalConfig.Ftp.link_http + remoteFilePath,
                            idalbum = idalbum,
                            thutu = i + 1,
                            TenAnh = contents[i],
                            createby = contextData.UserId
                        };
                        listAnh.Add(anh);
                    }
                    ftpClient.Disconnect();
                    var data = await _service.CreateAdrangeAsync(listAnh);
                    return Ok(data);
                }
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [AllowAnonymous]
        [HttpPost("upload-media")]
        public async Task<IActionResult> UploadMedia([FromForm] Anh_AlbumDtoCreate model, IFormFile thumbnail)
        {
            if (thumbnail == null || thumbnail.Length == 0)
                return BadRequest(new { Message = "No file uploaded." });

            // Định nghĩa các định dạng được phép
            var imageExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
            var videoExtensions = new[] { ".mp4", ".avi", ".mkv", ".mov", ".wmv" };

            var fileExtension = Path.GetExtension(thumbnail.FileName).ToLower();

            bool isImage = imageExtensions.Contains(fileExtension);
            bool isVideo = videoExtensions.Contains(fileExtension);

            if (!isImage && !isVideo)
                return BadRequest(new { Message = "Invalid file format. Only images or videos are allowed." });

            // Xác định thư mục theo loại file
            string subFolder = isImage ? "images" : "videos";
            string uniqueFileName = $"{Guid.NewGuid()}_{thumbnail.FileName}";


            // Tạo đường dẫn lưu file trên FTP
            string remoteDir = Path.Combine(GlobalConfig.Ftp.RemoteDirectory, subFolder).Replace("\\", "/");

            string remoteFilePath = Path.Combine(remoteDir, uniqueFileName).Replace("\\", "/");

            try
            {
                using (var ftpClient = new FtpClient
                {
                    Host = GlobalConfig.Ftp.Host,
                    Port = GlobalConfig.Ftp.Port,
                    Credentials = new NetworkCredential(GlobalConfig.Ftp.Username, GlobalConfig.Ftp.Password),
                    DataConnectionType = FtpDataConnectionType.AutoActive
                })
                {
                    // Kết nối
                    ftpClient.Connect();
                    // Upload file từ stream
                    using (var stream = thumbnail.OpenReadStream())
                    {
                        ftpClient.Upload(stream, remoteFilePath);
                    }

                    // Ngắt kết nối
                    ftpClient.Disconnect();
                }
                // Gán đường dẫn file vào model
                model.LinkAnh = GlobalConfig.Ftp.link_http + remoteFilePath;

                // Lưu dữ liệu
                var data = await _service.CreateAsync(model);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = "Error uploading file to FTP",
                    Error = ex.Message
                });
            }
        }
        /// <summary>
        /// ADmin: Thêm mới
        /// </summary>
        /// <param name="model">Thông tin </param>
        /// <param name="thumbnail">Ảnh đại diện </param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Anh_AlbumEntity>> InsertAsync([FromForm] Anh_AlbumDtoCreate model, IFormFile thumbnail)
        {
            try
            {
                if (thumbnail == null || thumbnail.Length == 0)
                    return BadRequest(new { Message = "No file uploaded." });
                var imageExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
                var videoExtensions = new[] { ".mp4", ".avi", ".mkv", ".mov", ".wmv" };

                var fileExtension = Path.GetExtension(thumbnail.FileName).ToLower();

                bool isImage = imageExtensions.Contains(fileExtension);
                bool isVideo = videoExtensions.Contains(fileExtension);

                if (!isImage && !isVideo)
                    return BadRequest(new { Message = "Invalid file format. Only images or videos are allowed." });

                // Xác định thư mục theo loại file
                string subFolder = isImage ? "images" : "videos";
                string uniqueFileName = $"{Guid.NewGuid()}_{thumbnail.FileName}";
                // Tạo đường dẫn lưu file trên FTP
                string remoteDir = Path.Combine(GlobalConfig.Ftp.RemoteDirectory, subFolder).Replace("\\", "/");
                string remoteFilePath = Path.Combine(remoteDir, uniqueFileName).Replace("\\", "/");

                using (var ftpClient = new FtpClient
                {
                    Host = GlobalConfig.Ftp.Host,
                    Port = GlobalConfig.Ftp.Port,
                    Credentials = new NetworkCredential(GlobalConfig.Ftp.Username, GlobalConfig.Ftp.Password),
                    DataConnectionType = FtpDataConnectionType.AutoActive
                })
                {
                    // Kết nối
                    ftpClient.Connect();
                    // Upload file từ stream
                    using (var stream = thumbnail.OpenReadStream())
                    {
                        ftpClient.Upload(stream, remoteFilePath);
                    }
                    // Ngắt kết nối
                    ftpClient.Disconnect();
                }
                // Gán đường dẫn file vào model
                model.LinkAnh = GlobalConfig.Ftp.link_http + remoteFilePath;
                var contextData = this.GetContext();
                model.createby = contextData.UserId;
                var data = await _service.CreateAsync(model);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Admin, Cổng: Lấy thông tin chi tiết của một thủ trưởng hiển thị để sửa...
        /// </summary>
        /// <param name="id"> id của bài viết</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var data = await _service.GetAsync(id);
            var dataupdate = ClassExtension.Map<Anh_AlbumDtoUpdate>(data);
            dataupdate.id = id;
            await _service.UpdateAsync(dataupdate);
            return Ok(data);
        }


        /// <summary>
        /// Admin: Lưu lại sau khi Sửa một ảnh cái này dùng postman cho dễ gòm id, thong tin album update dạng form, và file thumbnail
        /// </summary>
        /// <param name="id"> id </param>
        /// <returns>Trả về thông tin đã sửa</returns>
        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromForm] Anh_AlbumDtoUpdate model, [FromForm] IFormFile thumbnail)
        {
            if (id != model.id)
            {
                return BadRequest();
            }
            if (thumbnail != null)
            {
                var imageExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
                var videoExtensions = new[] { ".mp4", ".avi", ".mkv", ".mov", ".wmv" };

                var fileExtension = Path.GetExtension(thumbnail.FileName).ToLower();

                bool isImage = imageExtensions.Contains(fileExtension);
                bool isVideo = videoExtensions.Contains(fileExtension);

                if (!isImage && !isVideo)
                    return BadRequest(new { Message = "Invalid file format. Only images or videos are allowed." });

                // Xác định thư mục theo loại file
                string subFolder = isImage ? "images" : "videos";
                string uniqueFileName = $"{Guid.NewGuid()}_{thumbnail.FileName}";
                // Tạo đường dẫn lưu file trên FTP
                string remoteDir = Path.Combine(GlobalConfig.Ftp.RemoteDirectory, subFolder).Replace("\\", "/");
                string remoteFilePath = Path.Combine(remoteDir, uniqueFileName).Replace("\\", "/");

                using (var ftpClient = new FtpClient
                {
                    Host = GlobalConfig.Ftp.Host,
                    Port = GlobalConfig.Ftp.Port,
                    Credentials = new NetworkCredential(GlobalConfig.Ftp.Username, GlobalConfig.Ftp.Password),
                    DataConnectionType = FtpDataConnectionType.AutoActive
                })
                {
                    // Kết nối
                    ftpClient.Connect();
                    // Upload file từ stream
                    using (var stream = thumbnail.OpenReadStream())
                    {
                        ftpClient.Upload(stream, remoteFilePath);
                    }
                    // Ngắt kết nối
                    ftpClient.Disconnect();
                }
                // Gán đường dẫn file vào model
                model.LinkAnh = GlobalConfig.Ftp.link_http + remoteFilePath;
            }
            var contextData = this.GetContext();
            model.updateby = contextData.UserId;
            var data = await _service.UpdateAsync(model);
            return Ok(data);
        }
        /// <summary>
        /// Admin: Xoa ảnh trong album
        /// </summary>
        /// <param name="id"> id ảnh muốn xóa</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var contextData = this.GetContext();
            await _service.DeleteAsync(id);
            return Ok();
        }
    }
}
