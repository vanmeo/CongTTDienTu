using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Xim.Application.Contracts.DMThuTruong

{
    public class DMThuTruongDtoCreate
    {
        public int thutu { get; set; } = 1;

        public string Ma { get; set; }
        public string TenChucVu { get; set; }
        public string GhiChu { get; set; }
      
    }
}