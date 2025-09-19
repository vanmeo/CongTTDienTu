using Xim.Library.Attributes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Xim.Application.Contracts.Role
{
    public class RolePermisstionDtoCreate
    {
        public Guid role_id { get; set; }
        public Guid quyenhan_id { get; set; }
        public Guid module_id { get; set; }

        public bool is_chophep { get; set; }
    }
}
