using API.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Xim.Application.Contracts.CaiDat;
using Xim.Application.Contracts.Chucvus;
using Xim.Application.Contracts.NhatKy;
using Xim.Domain.Entities;
using Xim.Domain.Pagings;
using Xim.Library.Extensions;

namespace Xim.AppApi.Controllers
{
    [Authorize]
    public class CaiDatController : BaseController
    {
        /// <summary>
        /// Controller chứa các API liên quan tới thiết lập cài đặt cho hệ thống
        /// </summary>
        private readonly ICaiDatService _service;
        private readonly IWebHostEnvironment _environment;
        private readonly INhatKyService _serviceNhatKy;
        public CaiDatController(IServiceProvider serviceProvider, IWebHostEnvironment environment) : base(serviceProvider)
        {
            _service = serviceProvider.GetService<ICaiDatService>();
            _serviceNhatKy = serviceProvider.GetService<INhatKyService>();
            _environment = environment;
        }
        /// <summary>
        /// Lấy tất cả bản ghi cài đặt có phân trang, thực chất chỉ cần 1
        /// </summary>
        /// <param name="paging">Thông tin phân trang gồm</param>
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
       /// Admin: Chưa dùng tới đâu
       /// </summary>
       /// <param name="dto"></param>
       /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<CaiDatEntity>> InsertAsync([FromForm] CaiDatForm dto)
        {
            string folder = "CaiDat";
            string uploadPath = Path.Combine(_environment.WebRootPath, "Uploads", folder);
            Directory.CreateDirectory(uploadPath); // Tạo thư mục nếu chưa tồn tại
            var model = ClassExtension.Map<CaiDatDtoCreate>(dto);
            try
            {
                // Xử lý upload logo
                if (dto.logofile != null)
                {
                    string uniqueFileName = $"{Guid.NewGuid()}_{dto.logofile.FileName}";
                    string filePath = Path.Combine(uploadPath, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await dto.logofile.CopyToAsync(stream);
                    }

                    model.logo = $"/Uploads/{folder}/{uniqueFileName}";
                }

                // Xử lý upload icongioithieu
                if (dto.icongioithieufile != null)
                {
                    string uniqueFileName = $"{Guid.NewGuid()}_{dto.icongioithieufile.FileName}";
                    string filePath = Path.Combine(uploadPath, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await dto.icongioithieufile.CopyToAsync(stream);
                    }

                    model.icongioithieu = $"/Uploads/{folder}/{uniqueFileName}";
                }

                // Xử lý upload icon_lienhe
                if (dto.icon_lienhefile != null)
                {
                    string uniqueFileName = $"{Guid.NewGuid()}_{dto.icon_lienhefile.FileName}";
                    string filePath = Path.Combine(uploadPath, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await dto.icon_lienhefile.CopyToAsync(stream);
                    }

                    model.icon_lienhe = $"/Uploads/{folder}/{uniqueFileName}";
                }
                var contextData = this.GetContext();
                model.createby = contextData.UserId;
                // Gọi service để tạo mới dữ liệu
                var data = await _service.CreateAsync(model);
               await _serviceNhatKy.CreateAsync(new NhatKyDtoCreate { Bang="CaiDat",HoatDong="CREATE",MoTaHoatDong="Tạo mới cài đặt",TenNguoiDung= contextData.Username });
                return Ok(data);
            }
            catch (Exception ex)
            {
                // Log lỗi tại đây nếu cần thiết
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        /// <summary>
        /// Cổng: Lấy thông tin cài đặt để hiển thị lên cổng
        /// </summary>
        /// <param name="id">Mặc định: d3669e20-34f3-47d7-96ff-8de872d6a4fd</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var data = await _service.GetAsync(id);

            return Ok(data);
        }
        /// <summary>
        /// Admin: Cập nhật thông tin cài đặt
        /// </summary>
        /// <param name="id">d3669e20-34f3-47d7-96ff-8de872d6a4fd</param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromForm] CaiDatForm dto)
        {
            var caidat= await _service.GetAsync(id);
            // Đường dẫn tới thư mục lưu trữ
            var folderPath = Path.Combine(_environment.WebRootPath, "Uploads", "CaiDat");

            // Chỉ tạo thư mục nếu chưa tồn tại (CreateDirectory sẽ không tạo mới nếu thư mục đã có)
            Directory.CreateDirectory(folderPath);

          
            var model = ClassExtension.Map<CaiDatDtoUpdate>(dto);
            model.logo=caidat.logo;
            model.icongioithieu = caidat.icongioithieu;
            model.icon_lienhe = caidat.icon_lienhe;
            // Xử lý upload logo
            if (dto.logofile != null)
            {
                string uniqueFileName = $"{Guid.NewGuid()}_{dto.logofile.FileName}";
                string path = Path.Combine(folderPath, uniqueFileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await dto.logofile.CopyToAsync(stream);
                }

                model.logo = $"/Uploads/CaiDat/{uniqueFileName}";
            }

            // Xử lý upload icongioithieu
            if (dto.icongioithieufile != null)
            {
                string uniqueFileName = $"{Guid.NewGuid()}_{dto.icongioithieufile.FileName}";
                string path = Path.Combine(folderPath, uniqueFileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await dto.icongioithieufile.CopyToAsync(stream);
                }

                model.icongioithieu = $"/Uploads/CaiDat/{uniqueFileName}";
            }

            // Xử lý upload icon_lienhe
            if (dto.icon_lienhefile != null)
            {
                string uniqueFileName = $"{Guid.NewGuid()}_{dto.icon_lienhefile.FileName}";
                string path = Path.Combine(folderPath, uniqueFileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await dto.icon_lienhefile.CopyToAsync(stream);
                }

                model.icon_lienhe = $"/Uploads/CaiDat/{uniqueFileName}";
            }

           model.id=id ;

            var contextData = this.GetContext();
            model.updateby = contextData.UserId;
            var data = await _service.UpdateAsync(model);
            var v = new NhatKyDtoCreate { Bang = "CaiDat", HoatDong = "UPDATE", MoTaHoatDong = "Sửa bảng cài đặt", TenNguoiDung = contextData.Username };
            await _serviceNhatKy.CreateAsync(v);
            return Ok(data);
        }
        /// <summary>
        /// Admin: Xóa hiện tại không dùng đâu
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var contextData = this.GetContext();
            await _service.DeleteAsync(id);
            await _serviceNhatKy.CreateAsync(new NhatKyDtoCreate { Bang = "CaiDat", HoatDong = "DELETE", MoTaHoatDong = "Delete cài đặt", TenNguoiDung = contextData.Username });
            return Ok();
        }
    }

}
