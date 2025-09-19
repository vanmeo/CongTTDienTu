using System;
using System.Collections.Generic;
using System.Text;

namespace Xim.Domain.Entities
{
    public class QuyenHanEntity : BaseEntity<Guid>
    {
        public string ten { get; set; }
        public string ghichu { get; set; }
    }
}
