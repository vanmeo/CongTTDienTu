using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Xim.Application.Contracts.ThuTruongBQP

{
    public class ThuTruongBQPDtoCreate
    {
        public Guid? IdDMThuTruong { get; set; }
        public int? thutu { get; set; }
        public string? SoHieuQuanNhan { get; set; }
        public string? HoVaTen { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string? QueQuan { get; set; }
        public string? HocHamHV { get; set; }
        public string? TrinhDoCM { get; set; }
        public string? LyLuanCT { get; set; }

        public string? NgoaiNgu { get; set; }
        public string? CapBac { get; set; }
        public string? ChucVuDang { get; set; }
        public string? ChucVuCQ { get; set; }
        public string? TenLink { get; set; }
        public string? AnhChinh { get; set; }
        public string? NoiDungChinh { get; set; }
        public string? NoiDung { get; set; }
        public int? TinhTrang { get; set; }
        public string? GhiChu { get; set; }
        public int ViewCount { get; set; } = 0;
        public bool? is_deleted { get; set; }

        public Guid? createby { get; set; }
     
    }
}
