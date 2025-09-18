using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Xim.Application.Contracts.DMTailieu;
using Xim.Application.Contracts.TinTuc;
using Xim.Domain.Pagings;
using Xim.Library.Extensions;
using Xim.Storage;

namespace Xim.AppApi.Controllers
{
    [Authorize]
    public class DMTaiLieuController : BaseController
    {
        private readonly IDMTailieuService _service;
        private readonly IWebHostEnvironment _environment;

        private readonly IStorageService _storageService;
        internal const string DOWNLOAD_CONTENT_TYPE = "application/octect-stream";
        //        public FileController(IServiceProvider serviceProvider) : base(serviceProvider)
        //        {
        //            _storageService = serviceProvider.GetService<IStorageService>();
        //        }

        public DMTaiLieuController(IServiceProvider serviceProvider, IWebHostEnvironment environment) : base(serviceProvider)
        {
            _storageService = serviceProvider.GetService<IStorageService>();
            _service = serviceProvider.GetService<IDMTailieuService>();
            _environment = environment;
        }
      
        //[HttpGet("download")]
        //[AllowAnonymous]
        //public async Task<IActionResult> DownloadAsync(StorageCategory category, string name, string new_name)
        //{
        //    var stream = await _storageService.GetAsync(category, name);
        //    string fileName;
        //    if (!string.IsNullOrEmpty(new_name))
        //    {
        //        fileName = $"{Path.GetFileNameWithoutExtension(new_name)}{Path.GetExtension(name)}";
        //    }
        //    else
        //    {
        //        fileName = name;
        //    }

        //    return File(stream, DOWNLOAD_CONTENT_TYPE, fileName);
        //}



        /// <summary>
        /// CỔng: download file tài liệu
        /// </summary>
        /// <param name="id">id tài liệu</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("download")]
        public async Task<IActionResult> DownloadFile(Guid id)
        {
            var entity=await _service.GetAsync(id);
          
            if (string.IsNullOrEmpty(entity.filename))
            {
                return BadRequest("Filename is not provided.");
            }
            Console.WriteLine(_environment);

           Console.WriteLine(_environment.WebRootPath);
            // Xác định đường dẫn tuyệt đối của file
            var filePath = _environment.WebRootPath+entity.file_url.Replace(@"/",@"\");

            // Kiểm tra file có tồn tại không
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("File not found.");
            }

            // Lấy loại MIME (Media Type) dựa trên đuôi file
            var contentType = GetContentType(filePath);

            // Trả về file
            return PhysicalFile(filePath, contentType, entity.filename);
        }

        // Phương thức để xác định Content-Type
        private string GetContentType(string path)
        {
            var types = new Dictionary<string, string>
        {
            {".txt", "text/plain"},
            {".pdf", "application/pdf"},
            {".doc", "application/vnd.ms-word"},
            {".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document"},
            {".xls", "application/vnd.ms-excel"},
            {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
            {".png", "image/png"},
            {".jpg", "image/jpeg"},
            {".jpeg", "image/jpeg"},
            {".gif", "image/gif"},
            {".csv", "text/csv"}
        };

            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types.ContainsKey(ext) ? types[ext] : "application/octet-stream";
        }
        /// <summary>
        /// Admin: Lấy danh sách các tài liệu sắp xếp theo thời gian tạo
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("getall")]
        public async Task<IActionResult> GetListAsync(paging paging)
        {
            int offset = (paging.pageNumber - 1) * paging.pageSize;
            PagingParam param = new PagingParam();
            param.sort = "created";
            param.skip = offset;
            param.take = paging.pageSize;
            param.filter = "[{ 'f':'is_deleted','o':'=','v':'0'}]";
            param.columns = "";
            var data = await _service.GetListAsync(param);
            dynamic sumDataDynamic = data.sumData;

            // Truy cập thuộc tính total
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
        /// Admin: Thêm mới một tài liệu
        /// </summary>
        /// <param name="model"></param>
        /// <param name="fileTaiLieu">FIle đính kèm tài liệu</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> InsertAsync([FromForm] DMTailieuDtoCreate model, IFormFile fileTaiLieu)
        {
            if (fileTaiLieu != null && fileTaiLieu.Length > 0)
            {
                string uniqueFileName = $"{Guid.NewGuid()}_{fileTaiLieu.FileName}";
                var filePath = Path.Combine(_environment.WebRootPath,"Uploads", "FileDMTaiLieu", uniqueFileName);
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await fileTaiLieu.CopyToAsync(stream);
                }
                model.file_url = $"/Uploads/FileDMTaiLieu/{uniqueFileName}";
                model.filename= uniqueFileName;
            }
            var contextData = this.GetContext();
            model.createby = contextData.UserId;
            var data = await _service.CreateAsync(model);
            return Ok(data);
        }
        /// <summary>
        /// Admin,Cổng: Xem chi tiết một tài liệu phục vụ admin xem sửa, người dùng xem
        /// </summary>
        /// <param name="id">id tài liệu</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var data = await _service.GetAsync(id);
            data.ViewCount += 1;
            var dataupdate = ClassExtension.Map<DMTailieuDtoUpdate>(data);
            dataupdate.id = id;
            await _service.UpdateAsync(dataupdate);
            return Ok(data);
        }
        /// <summary>
        /// Admin: Lưu thông tin sửa tài liệu
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <param name="fileTaiLieu"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromForm] DMTailieuDtoUpdate model, [FromForm] IFormFile fileTaiLieu)
        {
            if (id != model.id)
            {
                return BadRequest();
            }
            if (fileTaiLieu != null && fileTaiLieu.Length > 0)
            {
                string uniqueFileName = $"{Guid.NewGuid()}_{fileTaiLieu.FileName}";
                var filePath = Path.Combine("Uploads", "FileDMTaiLieu", uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await fileTaiLieu.CopyToAsync(stream);
                }
                model.file_url = $"/Uploads/FileDMTaiLieu/{uniqueFileName}";
                model.filename = uniqueFileName;
            }
           
            var contextData = this.GetContext();
            model.ModifiedBy = contextData.UserId;
            var data = await _service.UpdateAsync(model);
            return Ok(data);
          
        }
        /// <summary>
        /// Admin: Xóa tài liệu
        /// </summary>
        /// <param name="id"></param>
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
