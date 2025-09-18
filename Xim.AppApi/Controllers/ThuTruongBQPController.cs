using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog.Filters;
using System.Reflection;
using Xim.Application.Contracts.ThuTruongBQP;
using Xim.Domain.Entities;
using Xim.Domain.Pagings;
using Xim.Library.Extensions;

namespace Xim.AppApi.Controllers
{
   [Authorize]
    public class ThuTruongBQPController : BaseController
    {
        private readonly IThuTruongBQPService _service;
        private readonly IWebHostEnvironment _environment;
        /// <summary>
        /// Admin: Phục vụ quản lý Thủ trưởng BQP
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="environment"></param>
        public ThuTruongBQPController(IServiceProvider serviceProvider, IWebHostEnvironment environment) : base(serviceProvider)
        {
            _service = serviceProvider.GetService<IThuTruongBQPService>();
            _environment = environment;
        }
        /// <summary>
        /// Admin: Liệt kê tất cả Thủ trưởng của VPB
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
        /// ADmin: Thêm mới
        /// </summary>
        /// <param name="model">Thông tin </param>
        /// <param name="thumbnail">Ảnh đại diện </param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ThuTruongBQPEntity>> InsertAsync([FromForm] ThuTruongBQPDtoCreate model, IFormFile thumbnail)
        {
            try
            {

                if (thumbnail != null && thumbnail.Length > 0)
                {
                    string uniqueFileName = $"{Guid.NewGuid()}_{thumbnail.FileName}";
                    // Đường dẫn tới thư mục lưu trữ
                    var folderPath = Path.Combine(_environment.WebRootPath, "Uploads", "ThuTruongBQP");

                    // Chỉ tạo thư mục nếu chưa tồn tại (CreateDirectory sẽ không tạo mới nếu thư mục đã có)
                    Directory.CreateDirectory(folderPath);

                    // Đường dẫn tệp đầy đủ
                    var filePath = Path.Combine(folderPath, uniqueFileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await thumbnail.CopyToAsync(stream);
                    }
                    model.AnhChinh = $"/Uploads/ThuTruongBQP/{uniqueFileName}";
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
 
            return Ok(data);
        }


        /// <summary>
        /// Admin: Lưu lại sau khi Sửa một Thủ trưởng này dùng postman cho dễ
        /// </summary>
        /// <param name="id"> id </param>
        /// <returns>Trả về thông tin đã sửa</returns>
        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromForm] ThuTruongBQPDtoUpdate model, [FromForm] IFormFile thumbnail)
        {
            if (id != model.id)
            {
                return BadRequest();
            }
            if (thumbnail != null)
            {
              
                string uniqueFileName = $"{Guid.NewGuid()}_{thumbnail.FileName}";
                // Đường dẫn tới thư mục lưu trữ
                var folderPath = Path.Combine(_environment.WebRootPath, "Uploads", "ThuTruongBQP");

                // Chỉ tạo thư mục nếu chưa tồn tại (CreateDirectory sẽ không tạo mới nếu thư mục đã có)
                Directory.CreateDirectory(folderPath);

                // Đường dẫn tệp đầy đủ
                var filePath = Path.Combine(folderPath, uniqueFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await thumbnail.CopyToAsync(stream);
                }
                model.AnhChinh = $"/Uploads/ThuTruongBQP/{uniqueFileName}";
            }
            var contextData = this.GetContext();
            model.updateby = contextData.UserId;
            var data = await _service.UpdateAsync(model);
            return Ok(data);
        }
        /// <summary>
        /// Admin: Xóa một người trong db
        /// </summary>
        /// <param name="id"> id Thủ trưởng muốn xóa</param>
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
