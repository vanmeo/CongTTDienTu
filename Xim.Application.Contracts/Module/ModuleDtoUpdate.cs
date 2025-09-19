using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Xim.Application.Contracts.Module
{
    public class ModuleDtoUpdate : ModuleDtoCreate
    {
        [JsonIgnore]
        public Guid id { get; set; }
    }
}
