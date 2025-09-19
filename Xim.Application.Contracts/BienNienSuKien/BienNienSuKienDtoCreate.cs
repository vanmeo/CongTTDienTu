using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Xim.Application.Contracts.BienNienSuKien

{
    public class BienNienSuKienDtoCreate
    {
        public string MocthoiGian { get; set; }
        public int thutu { get; set; } = 1;
        public string TieuDe { get; set; }

        public string AnhChinh { get; set; }
        //public string VideoChinh { get; set; }
        //public string ThoiLuong { get; set; }

       

        //public string NoiDungChinh { get; set; }
     
        //public string NoiDung { get; set; }
   
        public int TinhTrang { get; set; }
   
        public string GhiChu { get; set; }
        public Guid createby { get; set; }

    }
}