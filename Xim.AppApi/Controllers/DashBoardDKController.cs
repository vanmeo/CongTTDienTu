using API.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using System.Globalization;
using System.Reflection;
using System.Text.Json;
using Xim.Application.Contracts.DashBoardDK;
using Xim.Application.Contracts.NhatKy;
using Xim.Domain.Entities;
using Xim.Domain.Pagings;



namespace Xim.AppApi.Controllers
{
    [Authorize]
    public class DashBoardDKController : BaseController
    {
        private readonly IDashBoardDKService _service;
        private readonly IWebHostEnvironment _environment;
        private readonly INhatKyService _serviceNhatKy;
        /// <summary>
        /// Admin: Phục vụ quản lý Ảnh dashboard
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="environment"></param>
        public DashBoardDKController(IServiceProvider serviceProvider, IWebHostEnvironment environment) : base(serviceProvider)
        {
            _service = serviceProvider.GetService<IDashBoardDKService>();
            _environment = environment;
            _serviceNhatKy = serviceProvider.GetService<INhatKyService>();
        }
        /// <summary>
        /// Upload file excel theo mẫu chị gửi
        /// </summary>
        /// <param name="file">File excel theo mẫu</param>
        /// <param name="MotaDuLieu">Mô tả ví dụ như là file ngày giờ nào... sau dễ tìm</param>
        /// <returns></returns>
         [HttpPost("upload")]
        public async Task<IActionResult> UploadExcel(IFormFile file, [FromForm] string MotaDuLieu)
        {
            DashBoardDKDtoCreate dashBoardDKDtoCreate = new DashBoardDKDtoCreate();
            dashBoardDKDtoCreate.MoTaAnh = MotaDuLieu;

            if (file == null || file.Length == 0)
                return BadRequest("Chưa chọn file Excel");

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // Định dạng thư mục theo tháng-năm
            string monthYearFolder = DateTime.Now.ToString("MM-yyyy");
            string uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
            var folderPath = Path.Combine(_environment.WebRootPath, "Uploads", "imageDashBoardDK", monthYearFolder);

            Directory.CreateDirectory(folderPath);

            var filePath = Path.Combine(folderPath, uniqueFileName);

            // Ghi file xuống ổ đĩa
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Lưu URL để trả ra API
            dashBoardDKDtoCreate.Url_Anh = $"/Uploads/imageDashBoardDK/{monthYearFolder}/{uniqueFileName}";

            // ==== ĐỌC FILE EXCEL TỪ Ổ ĐĨA ====
            using var package = new ExcelPackage(new FileInfo(filePath));

            var ListDanhSachSuKien = new List<NhatKySuKien>();
            var DSTrangThaiTheoVung = new List<TrangThaiCacVung>();
            var DSKetQuaChung = new List<KetQuaChung>();
            var groupByVung = new Dictionary<string, List<TrangThaiCacVung>>();

            // Ví dụ đọc sheet 1 (index = 0)
            int sheetNhatKySuKien = 0;
            if (package.Workbook.Worksheets.Count > sheetNhatKySuKien)
            {
                var worksheetCapNhat = package.Workbook.Worksheets[sheetNhatKySuKien];
                int rowCount = worksheetCapNhat.Dimension.Rows;

                for (int row = 4; row <= rowCount; row++)
                {
                    string mocThoiGian = worksheetCapNhat.Cells[row, 2].Text?.Trim();
                    string noiDung = worksheetCapNhat.Cells[row, 3].Text?.Trim();

                    if (string.IsNullOrEmpty(mocThoiGian) && string.IsNullOrEmpty(noiDung))
                        break;

                    var rowData = new NhatKySuKien
                    {
                        ThoiGian = mocThoiGian ?? string.Empty,
                        NoiDung = noiDung ?? string.Empty
                    };

                    ListDanhSachSuKien.Add(rowData);
                }

                // Đảo ngược nếu muốn sự kiện mới nhất lên đầu
                ListDanhSachSuKien.Reverse();

                dashBoardDKDtoCreate.NhatKySuKien =
                    System.Text.Json.JsonSerializer.Serialize(ListDanhSachSuKien, new JsonSerializerOptions { WriteIndented = true });
            }

            // ... phần code đọc sheet khác giữ nguyên
            // chỉ cần thay stream = new FileStream(filePath, FileMode.Open, FileAccess.Read)

            var contextData = this.GetContext();
            dashBoardDKDtoCreate.createby = contextData.UserId;
            var data = await _service.CreateAsync(dashBoardDKDtoCreate);
            await _serviceNhatKy.CreateAsync(new NhatKyDtoCreate
            {
                Bang = "DashBoardDK",
                HoatDong = "CREATE",
                MoTaHoatDong = "Tạo mới Ảnh dashboard",
                TenNguoiDung = contextData.Username
            });

            return Ok(data);
        }


        //public async Task<IActionResult> UploadExcel(IFormFile file, [FromForm] string MotaDuLieu)
        //{
        //    DashBoardDKDtoCreate dashBoardDKDtoCreate = new DashBoardDKDtoCreate();
        //    dashBoardDKDtoCreate.MoTaAnh = MotaDuLieu;
        //    if (file == null || file.Length == 0)
        //        return BadRequest("Chưa chọn file Excel");
        //    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        //    // Đặt license cho EPPlus 8+

        //    // Định dạng thư mục theo tháng-năm
        //    string monthYearFolder = DateTime.Now.ToString("MM-yyyy");
        //    string uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
        //    // Đường dẫn tới thư mục lưu trữ
        //    var folderPath = Path.Combine(_environment.WebRootPath, "Uploads", "imageDashBoardDK", monthYearFolder);

        //    // Chỉ tạo thư mục nếu chưa tồn tại (CreateDirectory sẽ không tạo mới nếu thư mục đã có)
        //    Directory.CreateDirectory(folderPath);

        //    // Đường dẫn tệp đầy đủ
        //    var filePath = Path.Combine(folderPath, uniqueFileName);
        //    using (var stream = new FileStream(filePath, FileMode.Create))
        //    {
        //        await file.CopyToAsync(stream);
        //    }
        //    dashBoardDKDtoCreate.Url_Anh = $"/Uploads/imageDashBoardDK/{monthYearFolder}/{uniqueFileName}";

        //    using var stream = new MemoryStream();
        //    await file.CopyToAsync(stream);
        //    string CapNhatMoiNhat;
        //    using var package = new ExcelPackage(stream);
        //    var ListDanhSachSuKien = new List<NhatKySuKien>();
        //    var DSTrangThaiTheoVung = new List<TrangThaiCacVung>();
        //    var DSKetQuaChung = new List<KetQuaChung>();
        //    var groupByVung = new Dictionary<string, List<TrangThaiCacVung>>();
        //    // Chỉ đọc sheet thứ 3 (index = 3 là sheet thứ 4, nên cần sửa lại thành index 2)
        //    int sheetNhatKySuKien = 0; // sheet thứ 3 (0-based index)
        //    if (package.Workbook.Worksheets.Count > sheetNhatKySuKien)
        //    {
        //        //Doc Excel lấy thông tin cập nhật mới nhất
        //        var worksheetCapNhat = package.Workbook.Worksheets[sheetNhatKySuKien];
        //        int rowCount = worksheetCapNhat.Dimension.Rows;

        //        // Đọc dữ liệu từ dòng 4 trở đi
        //        for (int row = 4; row <= rowCount; row++)
        //        {
        //            string mocThoiGian = worksheetCapNhat.Cells[row, 2].Text?.Trim();
        //            string noiDung = worksheetCapNhat.Cells[row, 3].Text?.Trim();

        //            // Nếu cả 2 đều rỗng thì bỏ qua
        //            if (string.IsNullOrEmpty(mocThoiGian) && string.IsNullOrEmpty(noiDung))
        //                break;
        //            var rowData = new NhatKySuKien
        //            {
        //                ThoiGian = mocThoiGian ?? string.Empty,
        //                NoiDung = noiDung ?? string.Empty
        //            };

        //            ListDanhSachSuKien.Add(rowData);
        //            ListDanhSachSuKien.Reverse();
        //            CapNhatMoiNhat = System.Text.Json.JsonSerializer.Serialize(ListDanhSachSuKien, new JsonSerializerOptions
        //            {
        //                WriteIndented = true // để format đẹp, bỏ nếu muốn 1 dòng
        //            });
        //            dashBoardDKDtoCreate.NhatKySuKien = CapNhatMoiNhat;
        //        }
        //    }

        //    // Chỉ đọc sheet thứ 3 (index = 3 là sheet thứ 4, nên cần sửa lại thành index 2)
        //    int sheetTrangThaiAntoan = 1; // sheet thứ 3 (0-based index)
        //    if (package.Workbook.Worksheets.Count > sheetTrangThaiAntoan)
        //    {
        //        //Doc Excel lấy thông tin Trạng thái
        //        var worksheetCapNhat = package.Workbook.Worksheets[sheetTrangThaiAntoan];
        //        int rowCount = worksheetCapNhat.Dimension.Rows;

        //        for (int row = 5; row <= rowCount; row++)
        //        {
        //            string Vung = worksheetCapNhat.Cells[row, 2].Text?.Trim();
        //            string KhuVuc = worksheetCapNhat.Cells[row, 3].Text?.Trim();
        //            string TrangThai = worksheetCapNhat.Cells[row, 4].Text?.Trim();
        //            // Nếu cả 2 đều rỗng thì bỏ qua
        //            if (string.IsNullOrEmpty(Vung))
        //                break;
        //            var rowData = new TrangThaiCacVung
        //            {
        //                Vung = Vung,
        //                KhuVuc = KhuVuc,
        //                TrangThai = TrangThai
        //            };
        //            DSTrangThaiTheoVung.Add(rowData);
        //        }
        //        groupByVung = DSTrangThaiTheoVung.GroupBy(x => x.Vung)
        //                                .ToDictionary(
        //                                    g => g.Key,
        //                                    g => g.ToList()
        //                                );
        //        dashBoardDKDtoCreate.TrangThaiAntoanCacHT = System.Text.Json.JsonSerializer.Serialize(groupByVung, new JsonSerializerOptions
        //        {
        //            WriteIndented = true
        //        });
        //    }
        //    int sheetKetqua = 2; // sheet thứ 3
        //    if (package.Workbook.Worksheets.Count > sheetKetqua)
        //    {
        //        //Doc Excel lấy thông tin Trạng thái
        //        var worksheet = package.Workbook.Worksheets[sheetKetqua];
        //        int rowCount = worksheet.Dimension.Rows;
        //        for (int row = 3; row <= rowCount; row++)
        //        {
        //            string NoiDung = worksheet.Cells[row, 1].Text?.Trim();
        //            float? BenA = Convert.ToSingle(worksheet.Cells[row, 2].Text.Trim(), CultureInfo.InvariantCulture);
        //            float? BenB = Convert.ToSingle(worksheet.Cells[row, 3].Text.Trim(), CultureInfo.InvariantCulture);
        //            if (string.IsNullOrEmpty(NoiDung))
        //                break;
        //            var rowData = new KetQuaChung
        //            {
        //                NoiDung = NoiDung,
        //                KetQuaChiTiet = new KetQuaCuThe
        //                {
        //                    BenA = BenA,
        //                    BenB = BenB
        //                }
        //            };
        //            DSKetQuaChung.Add(rowData);
        //        }

        //        dashBoardDKDtoCreate.KetQuaDienTap = System.Text.Json.JsonSerializer.Serialize(DSKetQuaChung, new JsonSerializerOptions
        //        {
        //            WriteIndented = true
        //        });
        //    }

        //    int sheetKetquaVung1 = 3; // sheet thứ 3
        //    if (package.Workbook.Worksheets.Count > sheetKetqua)
        //    {
        //        var DSKetQuaVung1 = new List<KetQuaChung>();
        //        //Doc Excel lấy thông tin Trạng thái
        //        var worksheet = package.Workbook.Worksheets[sheetKetquaVung1];
        //        int rowCount = worksheet.Dimension.Rows;
        //        KetQuaVung12 vung1 = new KetQuaVung12();
        //        vung1.TrangThaiDienTap = worksheet.Cells[1, 1].Text?.Trim();
        //        for (int row = 3; row <= rowCount; row++)
        //        {
        //            string NoiDung = worksheet.Cells[row, 1].Text?.Trim();
        //            float? BenA = Convert.ToSingle(worksheet.Cells[row, 2].Text.Trim(), CultureInfo.InvariantCulture);
        //            float? BenB = Convert.ToSingle(worksheet.Cells[row, 3].Text.Trim(), CultureInfo.InvariantCulture);
        //            if (string.IsNullOrEmpty(NoiDung))
        //                break;
        //            var rowData = new KetQuaChung
        //            {
        //                NoiDung = NoiDung,
        //                KetQuaChiTiet = new KetQuaCuThe
        //                {
        //                    BenA = BenA,
        //                    BenB = BenB
        //                }
        //            };
        //            DSKetQuaVung1.Add(rowData);
        //        }
        //        vung1.KetQuachungs = DSKetQuaVung1;
        //        dashBoardDKDtoCreate.KetQuaVung1 = System.Text.Json.JsonSerializer.Serialize(vung1, new JsonSerializerOptions
        //        {
        //            WriteIndented = true
        //        });
        //    }
        //    int sheetKetquaVung2 = 4; // sheet thứ 3
        //    if (package.Workbook.Worksheets.Count > sheetKetqua)
        //    {
        //        var DSKetQuaVung2 = new List<KetQuaChung>();
        //        //Doc Excel lấy thông tin Trạng thái
        //        var worksheet = package.Workbook.Worksheets[sheetKetquaVung2];
        //        int rowCount = worksheet.Dimension.Rows;
        //        KetQuaVung12 vung2 = new KetQuaVung12();
        //        vung2.TrangThaiDienTap = worksheet.Cells[1, 1].Text?.Trim();
        //        for (int row = 3; row <= rowCount; row++)
        //        {
        //            string NoiDung = worksheet.Cells[row, 1].Text?.Trim();
        //            float? BenA = Convert.ToSingle(worksheet.Cells[row, 2].Text.Trim(), CultureInfo.InvariantCulture);
        //            float? BenB = Convert.ToSingle(worksheet.Cells[row, 3].Text.Trim(), CultureInfo.InvariantCulture);
        //            if (string.IsNullOrEmpty(NoiDung))
        //                break;
        //            var rowData = new KetQuaChung
        //            {
        //                NoiDung = NoiDung,
        //                KetQuaChiTiet = new KetQuaCuThe
        //                {
        //                    BenA = BenA,
        //                    BenB = BenB
        //                }
        //            };
        //            DSKetQuaVung2.Add(rowData);
        //        }
        //        vung2.KetQuachungs = DSKetQuaVung2;
        //        dashBoardDKDtoCreate.KetQuaVung2 = System.Text.Json.JsonSerializer.Serialize(vung2, new JsonSerializerOptions
        //        {
        //            WriteIndented = true
        //        });
        //    }
        //    int sheetKetquaVung3 = 5; // sheet thứ 6
        //    if (package.Workbook.Worksheets.Count > sheetKetqua)
        //    {

        //        //Doc Excel lấy thông tin Trạng thái
        //        var worksheet = package.Workbook.Worksheets[sheetKetquaVung3];
        //        int rowCount = worksheet.Dimension.Rows;
        //        KetQuaVung34 vung3 = new KetQuaVung34();
        //        vung3.TrangThaiDienTap = worksheet.Cells[1, 1].Text?.Trim();
        //        BieuDoChiTiet benA = new BieuDoChiTiet();
        //        benA.ketquachung = Convert.ToSingle(worksheet.Cells[3, 2].Text.Trim(), CultureInfo.InvariantCulture);
        //        benA.antoan = Convert.ToSingle(worksheet.Cells[4, 2].Text.Trim(), CultureInfo.InvariantCulture);
        //        benA.khongantoan = Convert.ToSingle(worksheet.Cells[5, 2].Text.Trim(), CultureInfo.InvariantCulture);
        //        BieuDoChiTiet benB = new BieuDoChiTiet();
        //        benA.ketquachung = Convert.ToSingle(worksheet.Cells[3, 3].Text.Trim(), CultureInfo.InvariantCulture);
        //        benA.antoan = Convert.ToSingle(worksheet.Cells[4, 3].Text.Trim(), CultureInfo.InvariantCulture);
        //        benA.khongantoan = Convert.ToSingle(worksheet.Cells[5, 3].Text.Trim(), CultureInfo.InvariantCulture);
        //        vung3.BenA = benA;
        //        vung3.BenB = benB;
        //        dashBoardDKDtoCreate.KetQuaVung3 = System.Text.Json.JsonSerializer.Serialize(vung3, new JsonSerializerOptions
        //        {
        //            WriteIndented = true
        //        });
        //    }
        //    int sheetKetquaVung4 = 6; // sheet thứ 7
        //    if (package.Workbook.Worksheets.Count > sheetKetqua)
        //    {

        //        //Doc Excel lấy thông tin Trạng thái
        //        var worksheet = package.Workbook.Worksheets[sheetKetquaVung4];
        //        int rowCount = worksheet.Dimension.Rows;
        //        KetQuaVung34 vung4 = new KetQuaVung34();
        //        vung4.TrangThaiDienTap = worksheet.Cells[1, 1].Text?.Trim();
        //        BieuDoChiTiet benA = new BieuDoChiTiet();
        //        benA.ketquachung = Convert.ToSingle(worksheet.Cells[3, 2].Text.Trim(), CultureInfo.InvariantCulture);
        //        benA.antoan = Convert.ToSingle(worksheet.Cells[4, 2].Text.Trim(), CultureInfo.InvariantCulture);
        //        benA.khongantoan = Convert.ToSingle(worksheet.Cells[5, 2].Text.Trim(), CultureInfo.InvariantCulture);
        //        BieuDoChiTiet benB = new BieuDoChiTiet();
        //        benA.ketquachung = Convert.ToSingle(worksheet.Cells[3, 3].Text.Trim(), CultureInfo.InvariantCulture);
        //        benA.antoan = Convert.ToSingle(worksheet.Cells[4, 3].Text.Trim(), CultureInfo.InvariantCulture);
        //        benA.khongantoan = Convert.ToSingle(worksheet.Cells[5, 3].Text.Trim(), CultureInfo.InvariantCulture);
        //        vung4.BenA = benA;
        //        vung4.BenB = benB;
        //        dashBoardDKDtoCreate.KetQuaVung4 = System.Text.Json.JsonSerializer.Serialize(vung4, new JsonSerializerOptions
        //        {
        //            WriteIndented = true
        //        });
        //    }
        //    var contextData = this.GetContext();
        //    dashBoardDKDtoCreate.createby = contextData.UserId;
        //    var data = await _service.CreateAsync(dashBoardDKDtoCreate);
        //    await _serviceNhatKy.CreateAsync(new NhatKyDtoCreate { Bang = "DashBoardDK", HoatDong = "CREATE", MoTaHoatDong = "Tạo mới Ảnh dashboard", TenNguoiDung = contextData.Username });
        //    return Ok(data);
        //}


        /// <summary>
        /// Son: lây tất cả bản ghi có group theo ngày Sơn lấy cái này để view ngày nhé
        /// </summary>
        /// <returns>Danh sách bản ghi theo ngày</returns>

        [AllowAnonymous]
        [HttpGet("getallgroubyDate")]

        public async Task<IActionResult> getallgroubyDate()
        {
            List<Dashboard> dashboards = new List<Dashboard>();
            var dsgetall = await _service.GetAsync();
            foreach (var data in dsgetall)
            {
                var dashboard = new Dashboard
                {
                    Created = data.created ?? DateTime.MinValue,
                    MoTaAnh = data.MoTaAnh,
                    NhatKySuKien = string.IsNullOrWhiteSpace(data.NhatKySuKien) ? new List<NhatKySuKien>()
    : JsonConvert.DeserializeObject<List<NhatKySuKien>>(data.NhatKySuKien),
                    TrangThaiAntoanCacHT = string.IsNullOrWhiteSpace(data.TrangThaiAntoanCacHT)
    ? new Dictionary<string, List<TrangThaiCacVung>>()
    : JsonConvert.DeserializeObject<Dictionary<string, List<TrangThaiCacVung>>>(data.TrangThaiAntoanCacHT),

                    KetQuaDienTap = string.IsNullOrWhiteSpace(data.KetQuaDienTap)
    ? new List<KetQuaChung>()
    : JsonConvert.DeserializeObject<List<KetQuaChung>>(data.KetQuaDienTap),

                    KetQuaVung1 = string.IsNullOrWhiteSpace(data.KetQuaVung1)
    ? null
    : JsonConvert.DeserializeObject<KetQuaVung12>(data.KetQuaVung1),

                    KetQuaVung2 = string.IsNullOrWhiteSpace(data.KetQuaVung2)
    ? null
    : JsonConvert.DeserializeObject<KetQuaVung12>(data.KetQuaVung2),

                    KetQuaVung3 = string.IsNullOrWhiteSpace(data.KetQuaVung3)
    ? null
    : JsonConvert.DeserializeObject<KetQuaVung34>(data.KetQuaVung3),

                    KetQuaVung4 = string.IsNullOrWhiteSpace(data.KetQuaVung4)
    ? null
    : JsonConvert.DeserializeObject<KetQuaVung34>(data.KetQuaVung4),
                };
                dashboards.Add(dashboard);

            }
            var grouped = dashboards
       .GroupBy(d => d.Created.Date) // group theo ngày, bỏ giờ phút
       .Select(g => new
       {
           Created = g.Key,
           Items = g.ToList()
       })
       .ToList();

            return Ok(grouped);
        }
        /// <summary>
        /// Hàm trả về một ảnh cuối cùng để đưa lên dashboard từ dữ liệu đọc từ excel dùng hàm này nhé Sơn 
        /// </summary>
        /// <returns>Trả về json dữ liệu view lên ẢNh </returns>
        [AllowAnonymous]
        [HttpGet("GetDashBoardfromDB")]
        public async Task<IActionResult> GetDashBoardfromDB()
        {
            var data = await _service.GetDashboard();
            var dashboard = new Dashboard
            {
                Created = data.created ?? DateTime.MinValue,
                MoTaAnh = data.MoTaAnh,
                NhatKySuKien = JsonConvert.DeserializeObject<List<NhatKySuKien>>(data.NhatKySuKien),
                TrangThaiAntoanCacHT = JsonConvert.DeserializeObject<Dictionary<string, List<TrangThaiCacVung>>>(data.TrangThaiAntoanCacHT),
                KetQuaDienTap = JsonConvert.DeserializeObject<List<KetQuaChung>>(data.KetQuaDienTap),
                KetQuaVung1 = JsonConvert.DeserializeObject<KetQuaVung12>(data.KetQuaVung1),
                KetQuaVung2 = JsonConvert.DeserializeObject<KetQuaVung12>(data.KetQuaVung2),
                KetQuaVung3 = JsonConvert.DeserializeObject<KetQuaVung34>(data.KetQuaVung3),
                KetQuaVung4 = JsonConvert.DeserializeObject<KetQuaVung34>(data.KetQuaVung4)
            };
            return Ok(dashboard);
        }
        /// <summary>
        /// Admin: Xóa một ẢNh trong db
        /// </summary>
        /// <param name="id"> id Ảnh muốn xóa</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var contextData = this.GetContext();
            await _service.DeleteAsync(id);
            return Ok();
        }

        /// <summary>
        /// Admin: Liệt kê tất cả ảnh trong mục dáhboard
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
        /// Thêm mới ảnh dashboard
        /// </summary>
        /// <param name="model">thông tin ảnh cần tạo không nhập gì cũng được sau tự gán link ảnh</param>
        /// <param name="thumbnail">Ảnh file</param>
        /// <returns></returns>
        [HttpPost]

        public async Task<ActionResult<DashBoardDKEntity>> InsertAsync([FromForm] DashBoardDKDtoCreate model, IFormFile thumbnail)
        {
            try
            {
                if (thumbnail != null && thumbnail.Length > 0)
                {
                    // Định dạng thư mục theo tháng-năm
                    string monthYearFolder = DateTime.Now.ToString("MM-yyyy");
                    string uniqueFileName = $"{Guid.NewGuid()}_{thumbnail.FileName}";
                    // Đường dẫn tới thư mục lưu trữ
                    var folderPath = Path.Combine(_environment.WebRootPath, "Uploads", "imageDashBoardDK", monthYearFolder);

                    // Chỉ tạo thư mục nếu chưa tồn tại (CreateDirectory sẽ không tạo mới nếu thư mục đã có)
                    Directory.CreateDirectory(folderPath);

                    // Đường dẫn tệp đầy đủ
                    var filePath = Path.Combine(folderPath, uniqueFileName);
                    Console.WriteLine(filePath);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await thumbnail.CopyToAsync(stream);
                    }
                    model.Url_Anh = $"/Uploads/imageDashBoardDK/{monthYearFolder}/{uniqueFileName}";
                }
                var contextData = this.GetContext();
                model.createby = contextData.UserId;
                var data = await _service.CreateAsync(model);
                await _serviceNhatKy.CreateAsync(new NhatKyDtoCreate { Bang = "DashBoardDK", HoatDong = "CREATE", MoTaHoatDong = "Tạo mới Ảnh dashboard", TenNguoiDung = contextData.Username });
                return Ok(data);
            }
            catch (Exception ex)
            {

                return Ok(ex);
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
        /// Hàm trả về một ảnh cuối cùng để đưa lên dashboard
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("GetDashBoard")]
        public async Task<IActionResult> GetDashBoard()
        {
            var data = await _service.GetDashboard();
            return Ok(data);
        }
        /// <summary>
        /// Update một ảnh giờ không dùng nữa thôi upload cái mới nhất thôi
        /// </summary>
        /// <param name="id">Id của ảnh</param>
        /// <param name="model">Thông tin ảnh</param>
        /// <param name="thumbnail">Ảnh mới</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromForm] DashBoardDKDtoUpdate model, [FromForm] IFormFile thumbnail)
        {
            if (thumbnail != null)
            {
                // Định dạng thư mục theo tháng-năm
                string monthYearFolder = DateTime.Now.ToString("MM-yyyy");
                string uniqueFileName = $"{Guid.NewGuid()}_{thumbnail.FileName}";
                // Đường dẫn tới thư mục lưu trữ
                var folderPath = Path.Combine(_environment.WebRootPath, "Uploads", "imageDashBoardDK", monthYearFolder);

                // Chỉ tạo thư mục nếu chưa tồn tại (CreateDirectory sẽ không tạo mới nếu thư mục đã có)
                Directory.CreateDirectory(folderPath);

                // Đường dẫn tệp đầy đủ
                var filePath = Path.Combine(folderPath, uniqueFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await thumbnail.CopyToAsync(stream);
                }
                model.Url_Anh = $"/Uploads/imageDashBoardDK/{monthYearFolder}/{uniqueFileName}";
            }
            var contextData = this.GetContext();
            model.updateby = contextData.UserId;
            var data = await _service.UpdateAsync(model);
            await _serviceNhatKy.CreateAsync(new NhatKyDtoCreate { Bang = "Dashboard", HoatDong = "UPDATE", MoTaHoatDong = "Update Dashboard", TenNguoiDung = contextData.Username });
            return Ok(data);
        }
        
    }
}
