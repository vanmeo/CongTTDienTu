using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Xim.Application.Contracts.Module;
using Xim.Domain.Pagings;

namespace Xim.AppApi.Controllers
{
    [Authorize]
    public class ModuleController : BaseController
    {
        /// <summary>
        /// Module phục vụ phân quyên người dùng
        /// </summary>
        private readonly IModuleService _service;
        public ModuleController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _service = serviceProvider.GetService<IModuleService>();
        }
        /// <summary>
        /// Admin: Liệt kê các module pv phân quyền
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
        /// Admin: Thêm mới module
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> InsertAsync([FromBody] ModuleDtoCreate model)
        {
            //var contextData = this.GetContext();
            //model.created_userid = contextData.UserId;
            var data = await _service.CreateAsync(model);
            return Ok(data);
        }
        /// <summary>
        /// CHi tiết một module
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
        /// Lưu thông tin sửa module
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] ModuleDtoUpdate model)
        {
            model.id = id;
            var data = await _service.UpdateAsync(model);
            return Ok(data);
        }
        /// <summary>
        /// Admin: Xóa module
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
