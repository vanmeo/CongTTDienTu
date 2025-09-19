using Xim.Library.Attributes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Xim.Application.Contracts.Role
{
    public class UserRoleDtoCreate
    {
        public List<Guid> Luser_id { get; set; }
        public Guid role_id { get; set; }

    }
}
