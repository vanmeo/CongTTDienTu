using Xim.Library.Attributes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Xim.Domain.Entities
{
    public class Anh_AlbumEntity : BaseEntity<Guid>, IEntityCreated
    {
        public Guid idalbum { get; set; }
        public int thutu { get; set; }
        public string TenAnh { get; set; }
      
        public string? LinkAnh { get; set; }
        public string? GhiChu { get; set; }
        public bool? is_deleted { get; set; }

        public Guid? createby { get; set; }
        public Guid? updateby { get; set; }
        public DateTime? created { get; set; }
        public DateTime? modified { get; set; }
        
    }
}
