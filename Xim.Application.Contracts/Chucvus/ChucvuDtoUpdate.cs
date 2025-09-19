using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Xim.Application.Contracts.Chucvus
{
    public class ChucvuDtoUpdate : ChucvuDtoCreate
    {
        [JsonIgnore]
        public Guid id { get; set; }
    }
}
