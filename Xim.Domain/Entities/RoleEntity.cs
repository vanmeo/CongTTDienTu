using System;
using System.Collections.Generic;
using System.Text;

namespace Xim.Domain.Entities
{
    public class RoleEntity : BaseEntity<Guid>
    {
        public string name { get; set; }
        public string ghichu { get; set; }
        public DateTime? created { get; set; }
    }
}
