using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Xim.Application.Contracts.MenuSub

{
    public class MenuSubDtoCreate
    {
        public string Ten { get; set; }
        public string GhiChu { get; set; }
        public Guid? parent_id { get; set; }
        public int thutu { get; set; }
    }
}
