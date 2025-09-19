using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Xim.Application.Contracts.NhatKy

{
    public class NhatKyDtoCreate
    {
        public string? TenNguoiDung { get; set; }
        public string? HoatDong { get; set; }
        public string? Bang { get; set; }
        public string? MoTaHoatDong { get; set; }
        public bool? is_deleted { get; set; }
        public Guid? createby { get; set; }
     
    }
}
