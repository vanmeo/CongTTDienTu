using Xim.Library.Attributes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Xim.Application.Contracts.Role
{
    public class RoleDtoUpdate
    {
        public Guid id { get; set; }        
        public List<Guid> lst_user_id { get; set; }

    }
}
