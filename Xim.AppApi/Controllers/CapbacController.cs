using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Xim.AppApi.Contexts;
using Xim.Application.Contracts.Capbacs;
using Xim.Application.Contracts.Donvis;
using Xim.Application.Contracts.NhatKy;
using Xim.Domain.Pagings;

namespace Xim.AppApi.Controllers
{
    [Authorize]
    public class CapbacController : BaseController
    {
        private readonly ICapbacService _service;
        private readonly ILogger<CapbacController> _logger;
        private readonly INhatKyService _serviceNhatKy;
        public CapbacController(IServiceProvider serviceProvider, ILogger<CapbacController> logger) : base(serviceProvider)
        {
            _service = serviceProvider.GetService<ICapbacService>();
            _logger = logger;
            _serviceNhatKy = serviceProvider.GetService<INhatKyService>();
        }
        /// <summary>
        /// Admin: Quản lý danh sách cấp bậc
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
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
        /// Admin: Thêm mới cấp bậc
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> InsertAsync([FromBody] CapbacDtoCreate model)
        {
            var contextData = this.GetContext();
  
            _logger.LogInformation("Index page visited at {date}", DateTime.UtcNow);
            _logger.LogDebug("debug message");
            var data = await _service.CreateAsync(model);
            await _serviceNhatKy.CreateAsync(new NhatKyDtoCreate { Bang = "CaiDat", HoatDong = "CREATE", MoTaHoatDong = "Tạo mới cấp bậc", TenNguoiDung = contextData.Username });
            return Ok(data);
        }
        /// <summary>
        /// Admin: Get by id
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
        /// Admin: Lưu thông tin sửa
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] CapbacDtoUpdate model)
        {
            var contextData = this.GetContext();
            model.id = id;
            var data = await _service.UpdateAsync(model);
            await _serviceNhatKy.CreateAsync(new NhatKyDtoCreate { Bang = "CapBac", HoatDong = "UPDATE", MoTaHoatDong = "update cấp bậc", TenNguoiDung = contextData.Username });
            return Ok(data);
        }
        /// <summary>
        /// Admin: Xóa thông tin sửa
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var contextData = this.GetContext();
            await _service.DeleteAsync(id);
            await _serviceNhatKy.CreateAsync(new NhatKyDtoCreate { Bang = "CapBac", HoatDong = "DELETE", MoTaHoatDong = "Delete cấp bậc", TenNguoiDung = contextData.Username });
            return Ok();
        }
    }
}
