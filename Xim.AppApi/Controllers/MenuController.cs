using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Xim.Application.Contracts.Menu;
using Xim.Domain.Pagings;

namespace Xim.AppApi.Controllers
{
    [Authorize]
    public class MenuController : BaseController
    {
        private readonly IMenuService _service;
        public MenuController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _service = serviceProvider.GetService<IMenuService>();
        }
        /// <summary>
        /// ADMIN: Lấy danh sách topmenu quản lý
        /// </summary>
        /// <param name="paging"> Gồm pagesize, page number</param>
        /// <returns>danh sách bài viết, thông tin phân trang</returns>
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
        /// ADMIN: Thêm mới một menu
        /// </summary>
        /// <param name="model">dữ liệu cho một menu</param>
        /// <returns>Bản ghi menu vừa tạo</returns>
        [HttpPost]
        public async Task<IActionResult> InsertAsync([FromBody] MenuDtoCreate model)
        {
            //var contextData = this.GetContext();
            //model.created_userid = contextData.UserId;
            var data = await _service.CreateAsync(model);
            return Ok(data);
        }
        /// <summary>
        ///Cổng: Lấy thông tin topmenu kèm cả submenu để hiển thị menu
        /// </summary>
        /// <returns>Danh sách topmenu</returns>
        [AllowAnonymous]
        [HttpGet("gettopMenu")]
        public async Task<IActionResult> GettopMenuAsync()
        {
            var data = await _service.GetListMenuAsync();
            return Ok(data);
        }
        /// <summary>
        /// Cổng: Lấy danh sách các tài liệu theo id menu có submenu 
        /// </summary>
        /// <param name="idmenu">id menu</param>
        /// <param name="paging">pagesize, pagenumber</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("GetTaiLieuBycosubMenu")]
        public async Task<IActionResult> GetTaiLieubyMenucosubAsync(Guid idmenu, paging paging)
        {
            var data = await _service.GetTaiLieubyMenucosubAsync(idmenu, paging);
            return Ok(data);
        }

        /// <summary>
        /// Cổng (Sơn): Lấy danh sách các tài liệu theo id menu
        /// </summary>
        /// <param name="idmenu">id menu</param>
        /// <param name="paging">pagesize, pagenumber</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("GetTaiLieuByMenu")]
        public async Task<IActionResult> GetTaiLieubyMenuAsync(Guid idmenu,paging paging)
        {
            var data = await _service.GetTaiLieubyMenuAsync(idmenu,paging);
            return Ok(data);
        }
        /// <summary>
        /// Cồng (Sơn): Lấy danh sách các bài viết theo id menu 
        /// </summary>
        /// <param name="idmenu">id menu</param>
        /// <param name="paging">pagesize, pagenumber</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("GetTinTucByMenu")]
        public async Task<IActionResult> GetTinTucbyMenuAsync(Guid idmenu, paging paging)
        {
            try
            {
                var data = await _service.GetTinBaibyMenuAsync(idmenu, paging);
                return Ok(data);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }
        /// <summary>
        /// Cồng: Lấy danh sách các bài viết theo id menu (có sub cổng không có dùng hàm trên) sẽ bao gồm các tin trong các submenu thuộc menu này
        /// </summary>
        /// <param name="idmenu">id menu</param>
        /// <param name="paging">pagesize, pagenumber</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("GetTinTucByMenucosub")]
        public async Task<IActionResult> GetTinTucbyMenucosubAsync(Guid idmenu, paging paging)
        {
            try
            {
                var data = await _service.GetTinBaibyMenucosubAsync(idmenu, paging);
                return Ok(data);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
           
        }
        /// <summary>
        /// Admin: lấy tin chi tiết của một menu theo id
        /// </summary>
        /// <param name="id">id menu</param>
        /// <returns>Chi tiết menu</returns>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var data = await _service.GetAsync(id);
            return Ok(data);
        }
        /// <summary>
        /// Admin lưu thông tin menu muốn sửa
        /// </summary>
        /// <param name="id">idmenu</param>
        /// <param name="model">Menu cần sửa</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] MenuDtoUpdate model)
        {
            model.id = id;
            var data = await _service.UpdateAsync(model);
            return Ok(data);
        }
        /// <summary>
        /// Admin xóa một menu
        /// </summary>
        /// <param name="id">id menu muốn xóa</param>
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
