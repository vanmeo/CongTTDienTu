using Xim.Library.Attributes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Xim.Domain.Entities
{
    public class TinTucEntity : BaseEntity<Guid>, IEntityCreated
    { 
        public string TieuDe { get; set; }
        public string TieuDeEN { get; set; }
        public string Link { get; set; }

        public string AnhChinh { get; set; }
        public string VideoChinh { get; set; }
        public string ThoiLuong { get; set; }

        public DateTime? NgayPH { get; set; }
        public string? TacGia { get; set; }
        public string NoiDungChinh { get; set; }
        public string? NoiDungChinhEN { get; set; }
        public string NoiDung { get; set; }
        public string? NoiDungDecode { get; set; }
        public string? NoiDungDecodeEN { get; set; }
       
        public int TinhTrang { get; set; }
        public bool? IsAnh { get; set; }
        public bool? IsVideo { get; set; }
        public bool? IsTaiLieu { get; set; }
        public string TuKhoa { get; set; }
        public string GhiChu { get; set; }
        public int ViewCount { get; set; } = 0;
       
        public Guid idmenu { get; set; }
        public Guid createby { get; set; }
        public Guid updateby { get; set; }
        public DateTime? created { get; set; }
        public DateTime? modified { get; set; }
        public bool? is_deleted { get; set; }
    }
}
