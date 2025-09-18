using System;
using System.Collections.Generic;
using System.Text;

namespace Xim.Domain.Entities
{
    public class ModulePermissionEntity
    {
        public string permission_id { get; set; }        
        public string module_id { get; set; }
        public int? indexs { get; set; }
    }
}
