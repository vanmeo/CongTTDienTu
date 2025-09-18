using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Xim.Application.Contracts.Menu
{
    public class MenuDtoUpdate : MenuDtoCreate
    {
        [JsonIgnore]
        public Guid id { get; set; }
    }
}
