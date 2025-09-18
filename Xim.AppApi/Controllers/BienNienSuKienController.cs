using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog.Filters;
using System.Reflection;
using Xim.Application.Contracts.BienNienSuKien;
using Xim.Domain.Entities;
using Xim.Domain.Pagings;
using Xim.Library.Extensions;

namespace Xim.AppApi.Controllers
{
  [Authorize]
    public class BienNienSuKienController : BaseController
    {
        private readonly IBienNienSuKienService _service;
        private readonly IWebHostEnvironment _environment;
        /// <summary>
        /// Admin: Phục vụ quản lý Biên niên sự kiện
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="environment"></param>
        public BienNienSuKienController(IServiceProvider serviceProvider, IWebHostEnvironment environment) : base(serviceProvider)
        {
            _service = serviceProvider.GetService<IBienNienSuKienService>();
            _environment = environment;
        }
        /// <summary>
        /// Admin: Liệt kê tất cả Biên niên sự kiện có trong DB
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
        /// ADmin: Thêm mới Biên niên sự kiện
        /// </summary>
        /// <param name="model">Thông tin </param>
        /// <param name="thumbnail">Ảnh đại diện </param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<BienNienSuKienEntity>> InsertAsync([FromForm] BienNienSuKienDtoCreate model, IFormFile thumbnail)
        {
            try
            {

                if (thumbnail != null && thumbnail.Length > 0)
                {
                    string uniqueFileName = $"{Guid.NewGuid()}_{thumbnail.FileName}";
                    // Đường dẫn tới thư mục lưu trữ
                    var folderPath = Path.Combine(_environment.WebRootPath, "Uploads", "BienNienSuKien");

                    // Chỉ tạo thư mục nếu chưa tồn tại (CreateDirectory sẽ không tạo mới nếu thư mục đã có)
                    Directory.CreateDirectory(folderPath);

                    // Đường dẫn tệp đầy đủ
                    var filePath = Path.Combine(folderPath, uniqueFileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await thumbnail.CopyToAsync(stream);
                    }
                    model.AnhChinh = $"/Uploads/BienNienSuKien/{uniqueFileName}";
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
        /// Admin, Cổng: Lấy thông tin của một bản biên niên sự kiện để sửa
        /// </summary>
        /// <param name="id"> id của bài viết</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var data = await _service.GetAsync(id);
            //data.ViewCount += 1;
            var dataupdate = ClassExtension.Map<BienNienSuKienDtoUpdate>(data);
            dataupdate.id = id;
            await _service.UpdateAsync(dataupdate);
            return Ok(data);
        }
     
        /// <summary>
        /// Admin: Lưu lại sau khi Sửa một Bài biên niên sự kiện này dùng postman cho dễ vì có ảnh thumbnail và các thoogn tin của Biên niên sự kiện
        /// </summary>
        /// <param name="id"> id </param>
        /// <returns>Trả về thông tin đã sửa</returns>
        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromForm] BienNienSuKienDtoUpdate model, [FromForm] IFormFile thumbnail)
        {
            if (id != model.id)
            {
                return BadRequest();
            }
            if (thumbnail != null)
            {
              
                string uniqueFileName = $"{Guid.NewGuid()}_{thumbnail.FileName}";
                // Đường dẫn tới thư mục lưu trữ
                var folderPath = Path.Combine(_environment.WebRootPath, "Uploads", "BienNienSuKien");

                // Chỉ tạo thư mục nếu chưa tồn tại (CreateDirectory sẽ không tạo mới nếu thư mục đã có)
                Directory.CreateDirectory(folderPath);

                // Đường dẫn tệp đầy đủ
                var filePath = Path.Combine(folderPath, uniqueFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await thumbnail.CopyToAsync(stream);
                }
                model.AnhChinh = $"/Uploads/BienNienSuKien/{uniqueFileName}";
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
