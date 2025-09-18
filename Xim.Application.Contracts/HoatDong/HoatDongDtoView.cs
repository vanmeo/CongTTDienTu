using System;
using System.Collections.Generic;
using System.Text;
using Xim.Application.Contracts.HoatDong;

namespace Xim.Application.Contracts.HoatDong
{
    public class HoatDongDtoView
    {
        public int ThuTu { get; set; }
        public string Url_Anh { get; set; }
        public string Mota { get; set; }

        public int? TinhTrang { get; set; }
        public string? MoTaHoatDong { get; set; }
        public Guid? updateby { get; set; }
    }
}
