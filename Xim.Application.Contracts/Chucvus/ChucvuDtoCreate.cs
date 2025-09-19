using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Xim.Application.Contracts.Chucvus
{
    public class ChucvuDtoCreate
    {
        public string ten { get; set; }
        public string viettat { get; set; }
        public int thutu { get; set; }
    }
}
