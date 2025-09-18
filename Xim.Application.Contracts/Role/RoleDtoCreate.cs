using Xim.Library.Attributes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Xim.Application.Contracts.Role
{
    public class RoleDtoCreate
    {
        public string name { get; set; }
        public string ghichu { get; set; }
        public List<Guid> lst_user_id { get; set; }

    }
}
