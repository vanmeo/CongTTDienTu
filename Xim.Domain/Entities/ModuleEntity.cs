using System;
using System.Collections.Generic;
using System.Text;

namespace Xim.Domain.Entities
{
    public class ModuleEntity : BaseEntity<Guid>
    {
        public string ten { get; set; }
        public string ma { get; set; }
        public string ghichu { get; set; }
    }
}
