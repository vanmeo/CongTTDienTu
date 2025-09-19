using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Xim.Application.Contracts.Capbacs

{
    public class CapbacDtoCreate
    {
        public string ten { get; set; }
        public string viettat { get; set; }
        public int thutu { get; set; }
    }
}
