using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Xim.Domain.Entities
{
    public class AlbumEntity : BaseEntity<Guid>, IEntityCreated
    {
        public int thutu { get; set; } = 1;
        public string Tenalbum {  get; set; }
        public string Link { get; set; }
        public string Icon { get; set; }
        public bool? is_video { get; set; }
        public bool? is_deleted { get; set; }
        public DateTime? created { get; set; }
        public DateTime? modified { get; set; }
        public Guid? createby { get; set; }
        public Guid? updateby { get; set; }


    }
}
