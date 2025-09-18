using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Xim.Application.Contracts.Menu

{
    public class MenuDtoCreate
    {
        public string Ten { get; set; }
        public string GhiChu { get; set; }
        public int iType { get; set; } = 1;
        public int thutu { get; set; } = 0;
    }
}
