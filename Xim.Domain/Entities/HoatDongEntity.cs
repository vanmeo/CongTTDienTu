using Xim.Library.Attributes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Xim.Domain.Entities
{
    public class HoatDongEntity : BaseEntity<Guid>, IEntityCreated
    {
        public int ThuTu { get; set; }
        public string Url_Anh { get; set; }
        public string Mota { get; set; }
       
        public int? TinhTrang { get; set; }
        public string? MoTaHoatDong { get; set; }
        public bool? is_deleted { get; set; }

        public Guid? createby { get; set; }
        public Guid? updateby { get; set; }
        public DateTime? created { get; set; }
        public DateTime? modified { get; set; }
        
    }
}
