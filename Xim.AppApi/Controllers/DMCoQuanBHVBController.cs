using API.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Xim.Application.Contracts.DMCoQuanBHVB;
using Xim.Application.Contracts.TinTuc;
using Xim.Domain.Entities;
using Xim.Domain.Pagings;
using Xim.Library.Extensions;

namespace Xim.AppApi.Controllers
{
    [Authorize]
    public class DMCoQuanBHVBController : BaseController
    {
        private readonly IDMCoQuanBHVBService _service;
        private readonly IWebHostEnvironment _environment;
        public DMCoQuanBHVBController(IServiceProvider serviceProvider, IWebHostEnvironment environment) : base(serviceProvider)
        {
            _service = serviceProvider.GetService<IDMCoQuanBHVBService>();
            _environment = environment;
        }
        /// <summary>
        /// Admin: Lấy danh sách Cơ quan ban hành văn bản
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        [HttpPost("getall")]
        public async Task<IActionResult> GetListAsync(paging paging)
        {
            int offset = (paging.pageNumber - 1) * paging.pageSize;
            PagingParam param = new PagingParam();
            param.sort = "created";
            param.skip = offset;
            param.take = paging.pageSize;
            param.filter = "";
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
        /// Admin: Thêm mới một cơ quan ban hành văn bản
        /// </summary>
        /// <param name="model"></param>
        /// <param name="thumbnail"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<DMCoQuanBHVBEntity>> InsertAsync([FromForm] DMCoQuanBHVBDtoCreate model, IFormFile thumbnail)
        {
            if (thumbnail != null && thumbnail.Length > 0)
            {
                string uniqueFileName = $"{Guid.NewGuid()}_{thumbnail.FileName}";
                var filePath = Path.Combine(_environment.WebRootPath, "Uploads", "CaiDat", uniqueFileName);
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await thumbnail.CopyToAsync(stream);
                }
                model.icon = $"/Uploads/CaiDat/{uniqueFileName}";
            }
            var contextData = this.GetContext();
            model.createby = contextData.UserId;
            var data = await _service.CreateAsync(model);
            return Ok(data);
        }
        /// <summary>
        /// Admin: Lấy chi tiết một cơ quan ban hành theo id để(xem chi tiết, sửa)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var data = await _service.GetAsync(id);
            return Ok(data);
        }
        /// <summary>
        /// Admin: Lưu thông tin sửa Cơ quan ban hành
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <param name="thumbnail"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromForm] DMCoQuanBHVBDtoUpdate model, IFormFile thumbnail)
        {
            if (thumbnail != null && thumbnail.Length > 0)
            {
                string uniqueFileName = $"{Guid.NewGuid()}_{thumbnail.FileName}";
                var filePath = Path.Combine("Uploads", "CaiDat", uniqueFileName);
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await thumbnail.CopyToAsync(stream);
                }
                model.icon = $"/Uploads/CaiDat/{uniqueFileName}";
            }
            var contextData = this.GetContext();
            model.updateby = contextData.UserId;
            var data = await _service.UpdateAsync(model);
            return Ok(data);
        }
        /// <summary>
        /// Admin: Xóa cơ quan ban hành
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
