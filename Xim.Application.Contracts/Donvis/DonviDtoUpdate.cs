using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Xim.Application.Contracts.Donvis
{
    public class DonviDtoUpdate : DonviDtoCreate
    {
        [JsonIgnore]
        public Guid id { get; set; }
    }
}
