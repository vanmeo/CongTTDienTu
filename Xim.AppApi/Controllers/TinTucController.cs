using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog.Filters;
using System.Reflection;
using Xim.Application.Contracts.NhatKy;
using Xim.Application.Contracts.TinTuc;
using Xim.Domain.Entities;
using Xim.Domain.Pagings;
using Xim.Library.Extensions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Xim.AppApi.Controllers
{
    [Authorize]
    public class TinTucController : BaseController
    {
        private readonly ITinTucService _service;
        private readonly IWebHostEnvironment _environment;
        private readonly INhatKyService _serviceNhatKy;
        /// <summary>
        /// Admin: Phục vụ quản lý tin tức
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="environment"></param>
        public TinTucController(IServiceProvider serviceProvider, IWebHostEnvironment environment) : base(serviceProvider)
        {
            _service = serviceProvider.GetService<ITinTucService>();
            _environment = environment;
            _serviceNhatKy = serviceProvider.GetService<INhatKyService>();
        }
        /// <summary>
        /// Admin: Liệt kê tất cả tin tức trong mục tin tức 
        /// </summary>
        /// <param name="paging">Phân trang gồm page size, page number</param>
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
        /// ADmin: Thêm mới 1 bài viết 
        /// </summary>
        /// <param name="model">Thông tin của một bài viết</param>
        /// <param name="thumbnail">Ảnh đại diện bài viết</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<TinTucEntity>> InsertAsync([FromForm] TinTucDtoCreate model, IFormFile thumbnail)
        {
            try
            {
                if (thumbnail != null && thumbnail.Length > 0)
                {
                    // Định dạng thư mục theo tháng-năm
                    string monthYearFolder = DateTime.Now.ToString("MM-yyyy");
                    string uniqueFileName = $"{Guid.NewGuid()}_{thumbnail.FileName}";
                    // Đường dẫn tới thư mục lưu trữ
                    var folderPath = Path.Combine(_environment.WebRootPath, "Uploads", "imagetintuc", monthYearFolder);

                    // Chỉ tạo thư mục nếu chưa tồn tại (CreateDirectory sẽ không tạo mới nếu thư mục đã có)
                    Directory.CreateDirectory(folderPath);

                    // Đường dẫn tệp đầy đủ
                    var filePath = Path.Combine(folderPath, uniqueFileName);
                    Console.WriteLine(filePath);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await thumbnail.CopyToAsync(stream);
                    }
                    model.AnhChinh = $"/Uploads/imagetintuc/{monthYearFolder}/{uniqueFileName}";
                }
                var contextData = this.GetContext();
                model.createby = contextData.UserId;
                var data = await _service.CreateAsync(model);
                await _serviceNhatKy.CreateAsync(new NhatKyDtoCreate { Bang = "TinTuc", HoatDong = "CREATE", MoTaHoatDong = "Tạo mới Tin tức", TenNguoiDung = contextData.Username });
                return Ok(data);
            }
            catch (Exception ex)
            {

                return Ok(ex);
            }
            
        }
        /// <summary>
        /// Admin, Cổng: Lấy thông tin chi tiết của một bài viết hiển thị để sửa...
        /// </summary>
        /// <param name="id"> id của bài viết</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var data = await _service.GetAsync(id);
            data.ViewCount += 1;
            var dataupdate = ClassExtension.Map<TinTucDtoUpdate>(data);
            dataupdate.id = id;
            await _service.UpdateAsync(dataupdate);
            return Ok(data);
        }
        /// <summary>
        /// Admin: Cổng upload file cho mục tin tức
        /// </summary>
        /// <param name="file">file</param>
        /// <returns>Ok trả về link của ảnh, lỗi thì trả về 404 do file không tồn tại</returns>
        [HttpPost("uploadImage")]
        [RequestSizeLimit(100000000)]
        public async Task<IActionResult> UploadAsync(IFormFile file)
        {
            try
            {
                if (file != null)
                {
                    // Định dạng thư mục theo tháng-năm
                    string monthYearFolder = DateTime.Now.ToString("MM-yyyy");
                    string uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";

                    // Đường dẫn tới thư mục lưu trữ
                    var folderPath = Path.Combine(_environment.WebRootPath, "Uploads", "image", monthYearFolder);

                    // Chỉ tạo thư mục nếu chưa tồn tại
                    Directory.CreateDirectory(folderPath);

                    // Đường dẫn đầy đủ của tệp
                    var filePath = Path.Combine(folderPath, uniqueFileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    // Cập nhật đường dẫn ảnh cho phản hồi
                    var link = $"/Uploads/image/{monthYearFolder}/{uniqueFileName}";
                    return Ok(new { imageUrl = link });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        /// <summary>
        /// Admin: Lưu lại sau khi Sửa một bài viết 
        /// </summary>
        /// <param name="id"> id của bài viết</param>
        /// <returns>Trả về bào viết đã sửa</returns>
        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromForm] TinTucDtoUpdate model, [FromForm] IFormFile thumbnail)
        {
            //if (id != model.id)
            //{
            //    return BadRequest();
            //}
            if (thumbnail != null)
            {
                // Định dạng thư mục theo tháng-năm
                string monthYearFolder = DateTime.Now.ToString("MM-yyyy");
                string uniqueFileName = $"{Guid.NewGuid()}_{thumbnail.FileName}";
                // Đường dẫn tới thư mục lưu trữ
                var folderPath = Path.Combine(_environment.WebRootPath, "Uploads", "imagetintuc", monthYearFolder);

                // Chỉ tạo thư mục nếu chưa tồn tại (CreateDirectory sẽ không tạo mới nếu thư mục đã có)
                Directory.CreateDirectory(folderPath);

                // Đường dẫn tệp đầy đủ
                var filePath = Path.Combine(folderPath, uniqueFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await thumbnail.CopyToAsync(stream);
                }
                model.AnhChinh = $"/Uploads/imagetintuc/{monthYearFolder}/{uniqueFileName}";
            }
            var contextData = this.GetContext();
            model.updateby = contextData.UserId;
            var data = await _service.UpdateAsync(model);
            await _serviceNhatKy.CreateAsync(new NhatKyDtoCreate { Bang = "TinTuc", HoatDong = "UPDATE", MoTaHoatDong = "Update Tin tức", TenNguoiDung = contextData.Username });
            return Ok(data);
        }
        /// <summary>
        /// Admin: Xóa một bài viết trong db
        /// </summary>
        /// <param name="id"> id bài viết muốn xóa</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var contextData = this.GetContext();
            await _service.DeleteAsync(id);
            await _serviceNhatKy.CreateAsync(new NhatKyDtoCreate { Bang = "TinTuc", HoatDong = "DELETE", MoTaHoatDong = "Delete Tin tức", TenNguoiDung = contextData.Username });
            return Ok();
        }
    }
}
