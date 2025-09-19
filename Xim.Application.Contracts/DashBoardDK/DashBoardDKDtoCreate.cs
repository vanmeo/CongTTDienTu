using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Xim.Application.Contracts.DashBoardDK

{
    public class DashBoardDKDtoCreate
    {
        public string? Url_Anh { get; set; }
        public string? MoTaAnh { get; set; }
        
        public string? NhatKySuKien { get; set; }
        public string? TrangThaiAntoanCacHT { get; set; }
        public string? KetQuaDienTap { get; set; }
        public string? KetQuaVung1 { get; set; }
        public string? KetQuaVung2 { get; set; }
        public string? KetQuaVung3 { get; set; }
        public string? KetQuaVung4 { get; set; }

        public bool? is_deleted { get; set; }

        public Guid? createby { get; set; }
     
    }
}
