using System;
using System.Collections.Generic;
using System.Text;

namespace Xim.Domain.Entities
{
    public class RolePermissionEntity : BaseEntity<Guid>
    {
        public Guid role_id { get; set; }
        public Guid quyenhan_id { get; set; }
        public Guid module_id { get; set; }
        public bool is_chophep { get; set; }
    }
}
