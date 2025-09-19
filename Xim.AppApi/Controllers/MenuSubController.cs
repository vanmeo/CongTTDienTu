using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Xim.Application.Contracts.MenuSub;
using Xim.Domain.Pagings;
using Xim.Application.Contracts.Menu;

namespace Xim.AppApi.Controllers
{
    [Authorize]
    public class MenuSubController : BaseController
    {
        private readonly IMenuSubService _service;
        private readonly IMenuService _menuservice;
        public MenuSubController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _service = serviceProvider.GetService<IMenuSubService>();
            _menuservice = serviceProvider.GetService<IMenuService>();
        }
        /// <summary>
        /// Admin: để quản lý danh sách các submenu
        /// </summary>
        /// <param name="paging">pagesize, page number</param>
        /// <returns>Danh sách có phân trang, thông tin phân trang</returns>
        [AllowAnonymous]
        [HttpPost("getall")]
        public async Task<IActionResult> GetListAsync(paging paging)
        {
            int offset = (paging.pageNumber - 1) * paging.pageSize;
            PagingParam param = new PagingParam();
            param.sort = "thutu";
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
        /// Cổng: hiển thị danh sách các tài liệu theo id submenu
        /// </summary>
        /// <param name="idsubmenu">idsubmenu</param>
        /// <param name="paging"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("GetTaiLieuBySubMenu")]
        public async Task<IActionResult> GetTaiLieubySubMenuAsync(Guid idsubmenu, paging paging)
        {
            var data = await _service.GetTaiLieubySubMenuAsync(idsubmenu, paging);
            return Ok(data);
        }
        /// <summary>
        /// Cổng: hiển thị danh sách các bài viết theo id submenu
        /// </summary>
        /// <param name="idsubmenu">idsubmenu</param>
        /// <param name="paging"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("GetTinTucBySubMenu")]
        public async Task<IActionResult> GetTinTucbySubMenuAsync(Guid idsubmenu, paging paging)
        {
            var data = await _service.GetTinBaibySubMenuAsync(idsubmenu, paging);
            return Ok(data);
        }
        /// <summary>
        /// Admin: Thêm mới một submenu
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> InsertAsync([FromBody] MenuSubDtoCreate model)
        {
            //var contextData = this.GetContext();
            //model.created_userid = contextData.UserId;
            var data = await _service.CreateAsync(model);
            return Ok(data);
        }
        /// <summary>
        /// Admin: Hiển thị thông tin chi tiết của một submenu
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Thông tin chi tiết của sub menu</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var data = await _service.GetAsync(id);
            return Ok(data);
        }
        /// <summary>
        /// Admin, CỔng: Lấy danh sách các submenu theo id topmenu
        /// </summary>
        /// <param name="menuid"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("getbymenu")]
        public async Task<IActionResult> GetbyMenuIdAsync(Guid menuid)
        {
            var data = await _service.GetbyMenuIdAsync(menuid);
            return Ok(data);
        }
        /// <summary>
        /// Admin: Lưu thông tin sửa của một submenu
        /// </summary>
        /// <param name="id">id sửa</param>
        /// <param name="model">Nội dung đã sửa</param>
        /// <returns>submenu đã sửa</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] MenuSubDtoUpdate model)
        {
            model.id = id;
            var data = await _service.UpdateAsync(model);
            return Ok(data);
        }
        /// <summary>
        /// Admin: Xóa submenu
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
