using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Xim.Application.Contracts.DMLinhvucVB

{
    public class DMLinhvucVBDtoCreate
    {
        public Guid? parent_id { get; set; }
        public string? ten { get; set; }
        public string? Description { get; set; }
        public string? Icon { get; set; }
        public bool? is_locked { get; set; }
        public bool? is_deleted { get; set; }
       
    }
}
