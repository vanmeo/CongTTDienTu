using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Xim.Application.Contracts.LienKet

{
    public class LienKetDtoCreate
    {
        public string ten { get; set; }
        public string? ma { get; set; }
        public string? link { get; set; }
        public string? logo_icon { get; set; }
        public int thutu { get; set; } = 0;
        public string? ghichu { get; set; }
    }
}
