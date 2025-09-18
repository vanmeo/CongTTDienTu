using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Xim.Application.Contracts.HoatDong

{
    public class HoatDongDtoCreate
    {
        public int ThuTu { get; set; }
        public string Url_Anh { get; set; }
        public string Mota { get; set; }

        public int? TinhTrang { get; set; }
        public string? MoTaHoatDong { get; set; }
        public bool? is_deleted { get; set; }


        //public Guid? updateby { get; set; }
        //public DateTime? created { get; set; }
        //public DateTime? modified { get; set; }

        public Guid? createby { get; set; }
     
    }
}
