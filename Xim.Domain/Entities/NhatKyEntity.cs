using Xim.Library.Attributes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Xim.Domain.Entities
{
    public class NhatKyEntity : BaseEntity<Guid>, IEntityCreated
    {
        public string? TenNguoiDung { get; set; }
        public string? HoatDong { get; set; }
        public string? Bang { get; set; }
       
        public string? MoTaHoatDong { get; set; }
        public bool? is_deleted { get; set; }

        public Guid? createby { get; set; }
        public Guid? updateby { get; set; }
        public DateTime? created { get; set; }
        public DateTime? modified { get; set; }
        
    }
}
