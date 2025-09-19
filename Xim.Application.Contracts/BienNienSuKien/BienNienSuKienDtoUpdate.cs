using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using Xim.Application.Contracts.BienNienSuKien;

namespace Xim.Application.Contracts.BienNienSuKien
{
    public class BienNienSuKienDtoUpdate : BienNienSuKienDtoCreate
    {
        [JsonIgnore]
        public Guid id { get; set; }
        public Guid updateby { get; set; }

    }
}
