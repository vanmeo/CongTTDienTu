using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Xim.Application.Contracts.DMLoaiVB

{
    public class DMLoaiVBDtoCreate
    {
        public string? ten { get; set; }
        public string? Description { get; set; }
        public string? Icon { get; set; }

        public int thutu { get; set; } = 0;
    }
}
