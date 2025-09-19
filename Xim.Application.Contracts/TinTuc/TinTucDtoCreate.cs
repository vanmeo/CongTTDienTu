using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Xim.Application.Contracts.TinTuc

{
    public class TinTucDtoCreate
    {
        public string TieuDe { get; set; }
        public Guid idmenu { get; set; }

        public string? AnhChinh { get; set; }

        public string? TacGia { get; set; }
        public string? NoiDungChinh { get; set; }
        public string? NoiDung { get; set; }
        public int TrangThai { get; set; }//1: duyệt; 0: không duyệt
        public string? TuKhoa { get; set; }
        public string? GhiChu { get; set; }

        public Guid? createby { get; set; }
     
    }
}
