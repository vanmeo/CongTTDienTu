using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Xim.Application.Contracts.Capbacs
{
    public class CapbacDtoUpdate : CapbacDtoCreate
    {
        [JsonIgnore]
        public Guid id { get; set; }
    }
}
