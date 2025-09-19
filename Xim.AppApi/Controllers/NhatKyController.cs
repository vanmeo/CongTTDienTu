using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog.Filters;
using System.Reflection;
using Xim.Application.Contracts.Donvis;
using Xim.Application.Contracts.NhatKy;
using Xim.Domain.Entities;
using Xim.Domain.Pagings;
using Xim.Library.Extensions;

namespace Xim.AppApi.Controllers
{
    [Authorize]
    public class NhatKyController : BaseController
    {
        private readonly INhatKyService _service;
        private readonly IWebHostEnvironment _environment;
        /// <summary>
        /// Admin: Phục vụ quản lý tin tức
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="environment"></param>
        public NhatKyController(IServiceProvider serviceProvider, IWebHostEnvironment environment) : base(serviceProvider)
        {
            _service = serviceProvider.GetService<INhatKyService>();
            _environment = environment;
        }
        /// <summary>
        /// Admin: Liệt kê tất cả ảnh trong mục hoạt động
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
        /// Admin: Thêm mới danh sách ảnh
        /// </summary>
        /// <param name="MotaAnh">Mô tả cho ảnh</param>
        /// <param name="thumbnails">Danh sách các ảnh upload lên</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> InsertAsync([FromBody] NhatKyDtoCreate model)
        {
            //var contextData = this.GetContext();
            //model.created_userid = contextData.UserId;

            var data = await _service.CreateAsync(model);
            return Ok(data);
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
            return Ok(data);
        }
        /// <summary>
        /// Admin: Lưu lại sau khi Sửa một bài viết 
        /// </summary>
        /// <param name="id"> id của bài viết</param>
        /// <returns>Trả về bào viết đã sửa</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] NhatKyDtoUpdate model)
        {
            model.id = id;
            var data = await _service.UpdateAsync(model);
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
            return Ok();
        }
    }
}
