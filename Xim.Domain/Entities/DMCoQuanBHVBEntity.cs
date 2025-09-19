using System;
using System.Collections.Generic;
using System.Text;

namespace Xim.Domain.Entities
{
    public class DMCoQuanBHVBEntity : BaseEntity<Guid>, IEntityCreated
    {
        public Guid? id_parent { get; set; }
        public string? ten { get; set; }
        public string? description { get; set; }
        public string? icon { get; set; }
        public int thutu { get; set; } = 0;
        public Guid? createby { get; set; }
        public Guid? updateby { get; set; }
        public DateTime? created { get; set; }
        public DateTime? updated { get; set; }

    }
}
