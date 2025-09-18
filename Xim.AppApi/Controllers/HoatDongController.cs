using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog.Filters;
using System.Reflection;
using Xim.AppApi.Contexts;
using Xim.Application.Contracts.HoatDong;
using Xim.Application.Contracts.NhatKy;
using Xim.Application.Contracts.TinTuc;
using Xim.Domain.Entities;
using Xim.Domain.Pagings;
using Xim.Library.Extensions;

namespace Xim.AppApi.Controllers
{
    [Authorize]
    public class HoatDongController : BaseController
    {
        private readonly IHoatDongService _service;
        private readonly IWebHostEnvironment _environment;
        private readonly INhatKyService _serviceNhatKy;
        /// <summary>
        /// Admin: Phục vụ quản lý tin tức
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="environment"></param>
        public HoatDongController(IServiceProvider serviceProvider, IWebHostEnvironment environment) : base(serviceProvider)
        {
            _service = serviceProvider.GetService<IHoatDongService>();
            _environment = environment;
            _serviceNhatKy = serviceProvider.GetService<INhatKyService>();
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
        public async Task<ActionResult> InsertAsync([FromForm] string MotaAnh, [FromForm] string MotaHoatdong, IList<IFormFile> thumbnails)
        {
            var contextData = this.GetContext();
            List<HoatDongDtoView> data = new List<HoatDongDtoView>();
            try
            {
              
                if (thumbnails != null && thumbnails.Count > 0)
                {
                   

                    // Định dạng thư mục theo tháng-năm
                    string monthYearFolder = DateTime.Now.ToString("MM-yyyy");

                    // Tạo danh sách để lưu các đường dẫn ảnh
                    List<string> imagePaths = new List<string>();

                    foreach (var thumbnail in thumbnails)
                    {
                        if (thumbnail.Length > 0)
                        {
                            var hoatdong = new HoatDongDtoCreate();
                            // Tạo tên file duy nhất
                            string uniqueFileName = $"{Guid.NewGuid()}_{thumbnail.FileName}";
                            // Đường dẫn tới thư mục lưu trữ
                            var folderPath = Path.Combine(_environment.WebRootPath, "Uploads", "imageHoatDong", monthYearFolder);

                            // Chỉ tạo thư mục nếu chưa tồn tại
                            Directory.CreateDirectory(folderPath);

                            // Đường dẫn tệp đầy đủ
                            var filePath = Path.Combine(folderPath, uniqueFileName);
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await thumbnail.CopyToAsync(stream);
                            }
                            hoatdong.MoTaHoatDong = MotaHoatdong;
                            hoatdong.Mota = MotaAnh;
                            hoatdong.is_deleted = false;
                            hoatdong.createby = contextData.UserId;
                            hoatdong.Url_Anh = $"/Uploads/imageHoatDong/{monthYearFolder}/{uniqueFileName}";
                            var hd = await _service.CreateAsync(hoatdong);
                            data.Add(hd);
                        }
                    }
                 

                }
                await _serviceNhatKy.CreateAsync(new NhatKyDtoCreate { Bang = "HoatDong", HoatDong = "INSERT", MoTaHoatDong = "Insert Hoạt động", TenNguoiDung = contextData.Username });
                return Ok(data);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }   
         
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
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromForm] string Motaanh, [FromForm] string MotaHoatdong, [FromForm] IFormFile thumbnail)
        {
            try
            {
                var v = await _service.GetAsync(id);

                if (v != null)
                {

                    var dataupdate = ClassExtension.Map<HoatDongDtoUpdate>(v);
                    dataupdate.MoTaHoatDong = MotaHoatdong;
                    dataupdate.Mota = Motaanh;
                    dataupdate.is_deleted = false;

                    if (thumbnail != null)
                    {
                        // Định dạng thư mục theo tháng-năm
                        string monthYearFolder = DateTime.Now.ToString("MM-yyyy");
                        string uniqueFileName = $"{Guid.NewGuid()}_{thumbnail.FileName}";
                        // Đường dẫn tới thư mục lưu trữ
                        var folderPath = Path.Combine(_environment.WebRootPath, "Uploads", "imageHoatDong", monthYearFolder);

                        // Chỉ tạo thư mục nếu chưa tồn tại (CreateDirectory sẽ không tạo mới nếu thư mục đã có)
                        Directory.CreateDirectory(folderPath);

                        // Đường dẫn tệp đầy đủ
                        var filePath = Path.Combine(folderPath, uniqueFileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await thumbnail.CopyToAsync(stream);
                        }

                        dataupdate.Url_Anh = $"/Uploads/imageHoatDong/{monthYearFolder}/{uniqueFileName}";
                    }
                    var contextData = this.GetContext();
                    dataupdate.updateby = contextData.UserId;
                    dataupdate.id = id;
                    var data = await _service.UpdateAsync(dataupdate);
                    await _serviceNhatKy.CreateAsync(new NhatKyDtoCreate { Bang = "HoatDong", HoatDong = "UPDATE", MoTaHoatDong = "Update Hoạt động", TenNguoiDung = contextData.Username });
                    return Ok(data);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
           
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
            await _serviceNhatKy.CreateAsync(new NhatKyDtoCreate { Bang = "HoatDong", HoatDong = "DELETE", MoTaHoatDong = "DELETE Hoạt động", TenNguoiDung = contextData.Username });
            return Ok();
        }
    }
}
