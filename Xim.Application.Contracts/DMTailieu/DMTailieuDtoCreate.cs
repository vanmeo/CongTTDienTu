using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Xim.Application.Contracts.DMTailieu

{
    public class DMTailieuDtoCreate
    {
        public string file_url { get; set; }
        public Guid idmenu { get; set; }
        public string? title { get; set; }
        public string? summary { get; set; }
        public string? soVB { get; set; }
        public DateTime? NgayBH { get; set; }
        public string? Nguoiky { get; set; }
        public Guid? ID_CoquanBH { get; set; }
        public Guid? ID_LoaiVB { get; set; }
        public Guid? ID_LinhVuc { get; set; }
        public DateTime? NgayHieuLuc { get; set; }
        public int ViewCount { get; set; } = 0;
        public string? filename { get; set; }
        public bool? is_deleted { get; set; }
        public Guid? createby { get; set; }
    }
}
