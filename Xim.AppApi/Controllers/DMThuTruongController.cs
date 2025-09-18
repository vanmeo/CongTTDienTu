using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Xim.Application.Contracts.DMThuTruong;
using Xim.Domain.Pagings;

namespace Xim.AppApi.Controllers
{
    [Authorize]
    public class DMThuTruongController : BaseController
    {
        private readonly IDMThuTruongService _service;
        public DMThuTruongController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _service = serviceProvider.GetService<IDMThuTruongService>();
        }
        /// <summary>
        /// ADMIN: Lấy danh sách chức vụ hiển thị trên bảng thủ trưởng: Chánh văn phòng, Phó chánh....
        /// </summary>
        /// <param name="paging"> Gồm pagesize, page number</param>
        /// <returns>danh sách Chức vụ, thông tin phân trang</returns>
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
        /// ADMIN: Thêm mới một DMThuTruong
        /// </summary>
        /// <param name="model">dữ liệu cho một DMThuTruong</param>
        /// <returns>Bản ghi DMThuTruong vừa tạo</returns>
        [HttpPost]
        public async Task<IActionResult> InsertAsync([FromBody] DMThuTruongDtoCreate model)
        {
            //var contextData = this.GetContext();
            //model.created_userid = contextData.UserId;
            var data = await _service.CreateAsync(model);
            return Ok(data);
        }
      
      
        /// <summary>
        /// Cổng (Sơn): Lấy danh sách các các thủ trưởng theo chức vụ ví dụ như Chánh văn phòng gồm những ai giai đoạn nào...
        /// </summary>
        /// <param name="idDMThuTruong">id DMThuTruong</param>
        /// <param name="paging">pagesize, pagenumber</param>
        /// <returns>Danh sách gồm chức vụ và DS Thủ trưởng</returns>
        [AllowAnonymous]
        [HttpPost("GetDSThuTruongByDmThuTruong")]
        public async Task<IActionResult> GetThuTruongByChucVuAsync(Guid idDMThuTruong,paging paging)
        {
            var data = await _service.GetThuTruongByChuVuAsync(idDMThuTruong,paging);
            return Ok(data);
        }
       
       
        /// <summary>
        /// Admin: lấy tin chi tiết của một DMThuTruong theo id
        /// </summary>
        /// <param name="id">id DMThuTruong</param>
        /// <returns>Chi tiết DMThuTruong</returns>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var data = await _service.GetAsync(id);
            return Ok(data);
        }
        /// <summary>
        /// Admin lưu thông tin DMThuTruong muốn sửa
        /// </summary>
        /// <param name="id">idDMThuTruong</param>
        /// <param name="model">DMThuTruong cần sửa</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] DMThuTruongDtoUpdate model)
        {
            model.id = id;
            var data = await _service.UpdateAsync(model);
            return Ok(data);
        }
        /// <summary>
        /// Admin xóa một DMThuTruong ví dụ Chánh văn phòng....
        /// </summary>
        /// <param name="id">id DMThuTruong muốn xóa</param>
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
