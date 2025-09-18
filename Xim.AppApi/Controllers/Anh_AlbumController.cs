using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog.Filters;
using System.Reflection;
using Xim.Application.Contracts.Anh_Album;
using Xim.Application.Contracts.HoatDong;
using Xim.Application.Contracts.NhatKy;
using Xim.Domain.Entities;
using Xim.Domain.Pagings;
using Xim.Library.Extensions;
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

                var listAnh = new List<Anh_AlbumDtoCreate>();
                var contextData = this.GetContext();

                for (int i = 0; i < files.Count; i++)
                {
                    var file = files[i];
                    var content = contents[i];
                    string uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
                    // Đường dẫn tới thư mục lưu trữ
                    var folderPath = Path.Combine(_environment.WebRootPath, "Uploads", "Anh_Album");

                    // Chỉ tạo thư mục nếu chưa tồn tại (CreateDirectory sẽ không tạo mới nếu thư mục đã có)
                    Directory.CreateDirectory(folderPath);

                    // Đường dẫn tệp đầy đủ
                    var filePath = Path.Combine(folderPath, uniqueFileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    var anh = new Anh_AlbumDtoCreate
                    {
                        LinkAnh = $"/Uploads/Anh_Album/{uniqueFileName}",
                        idalbum = idalbum,
                        thutu = i + 1,
                        TenAnh = contents[i],
                        createby = contextData.UserId
                    };
                    listAnh.Add(anh);
                }

                var data = await _service.CreateAdrangeAsync(listAnh);
                return Ok(data);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
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

                if (thumbnail != null && thumbnail.Length > 0)
                {
                    string uniqueFileName = $"{Guid.NewGuid()}_{thumbnail.FileName}";
                    // Đường dẫn tới thư mục lưu trữ
                    var folderPath = Path.Combine(_environment.WebRootPath, "Uploads", "Anh_Album");

                    // Chỉ tạo thư mục nếu chưa tồn tại (CreateDirectory sẽ không tạo mới nếu thư mục đã có)
                    Directory.CreateDirectory(folderPath);
                    
                    // Đường dẫn tệp đầy đủ

                    var filePath = Path.Combine(folderPath, uniqueFileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await thumbnail.CopyToAsync(stream);
                    }
                    model.LinkAnh = $"/Uploads/Anh_Album/{uniqueFileName}";
                }
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

                string uniqueFileName = $"{Guid.NewGuid()}_{thumbnail.FileName}";
                // Đường dẫn tới thư mục lưu trữ
                var folderPath = Path.Combine(_environment.WebRootPath, "Uploads", "Anh_Album");

                // Chỉ tạo thư mục nếu chưa tồn tại (CreateDirectory sẽ không tạo mới nếu thư mục đã có)
                Directory.CreateDirectory(folderPath);

                // Đường dẫn tệp đầy đủ
                var filePath = Path.Combine(folderPath, uniqueFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await thumbnail.CopyToAsync(stream);
                }
                model.LinkAnh = $"/Uploads/Anh_Album/{uniqueFileName}";
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
