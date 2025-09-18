using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Xim.Domain.Entities
{
    public class LienKetEntity : BaseEntity<Guid>, IEntityCreated
    {
        public string ten { get; set; }
        public string? ma { get; set; }
        public string? link { get; set; }
        public string? logo_icon { get; set; }
        public int thutu { get; set; } = 0;
        public Guid? createby { get; set; }
        public DateTime? created { get; set; }
        public string? ghichu { get; set; }


    }
}
