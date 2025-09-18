using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Xim.Application.Contracts.MenuSub
{
    public class MenuSubDtoUpdate : MenuSubDtoCreate
    {
        [JsonIgnore]
        public Guid id { get; set; }
    }
}
