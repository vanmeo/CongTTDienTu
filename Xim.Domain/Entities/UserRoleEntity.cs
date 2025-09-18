using System;
using System.Collections.Generic;
using System.Text;

namespace Xim.Domain.Entities
{
    public class UserRoleEntity : BaseEntity<Guid>
    {
        public Guid user_id { get; set; }
        public Guid role_id { get; set; }
    }
}
