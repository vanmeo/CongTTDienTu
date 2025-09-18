using System;
using System.Collections.Generic;
using System.Text;

namespace Xim.Domain.Entities
{
    public class DMTailieuEntity : BaseEntity<Guid>, IEntityCreated
    {
        public string file_url { get; set; }
        public Guid idmenu { get; set; }
        public int TrangThai { get; set; }
        public string? title { get; set; }
        public string? summary { get; set; }
        public string? soVB { get; set; }
        public DateTime? NgayBH { get; set; }
        public string? Nguoiky { get; set; }
        public string? filename {  get; set; }
        public Guid? ID_CoquanBH { get; set; }
        public Guid? ID_LoaiVB { get; set; }
        public Guid? ID_LinhVuc { get; set; }
        public DateTime? NgayHieuLuc { get; set; }
        public int ViewCount { get; set; }
        public DateTime? modified{ get; set; }
        public bool? is_deleted { get; set; }
        public DateTime? created { get; set; }
        public Guid? createby { get; set; }
        public Guid? updateby { get; set; }
    }
}
