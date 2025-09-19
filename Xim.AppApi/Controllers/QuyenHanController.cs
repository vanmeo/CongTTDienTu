using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Xim.Application.Contracts.QuyenHan;
using Xim.Domain.Pagings;

namespace Xim.AppApi.Controllers
{
    [Authorize]
    public class QuyenHanController : BaseController
    {
        /// <summary>
        /// Quản lý danh mục quyền hạn cho người dùng: Thêm sửa xóa, xem...
        /// </summary>
        private readonly IQuyenHanService _service;
        public QuyenHanController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _service = serviceProvider.GetService<IQuyenHanService>();
        }
        /// <summary>
        /// Admin: Liệt kê các quyền
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        [HttpPost("getall")]
        public async Task<IActionResult> GetListAsync(paging paging)
        {
            int offset = (paging.pageNumber - 1) * paging.pageSize;
            PagingParam param = new PagingParam();
            param.sort = "";
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
        ///Admin: Thêm mới quyền
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<IActionResult> InsertAsync([FromBody] QuyenHanDtoCreate model)
        {
            //var contextData = this.GetContext();
            //model.created_userid = contextData.UserId;
            var data = await _service.CreateAsync(model);
            return Ok(data);
        }
        /// <summary>
        /// Admin: Lấy thông tin chi tiết của một quyền
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
        ///Admin: Lưu thông tin sửa quyền
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] QuyenHanDtoUpdate model)
        {
            model.id = id;
            var data = await _service.UpdateAsync(model);
            return Ok(data);
        }
        /// <summary>
        /// Admin: Xóa thông tin quyền
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
