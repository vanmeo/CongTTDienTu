using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Xim.Application.Contracts.LienKet
{
    public class LienKetDtoUpdate : LienKetDtoCreate
    {
        [JsonIgnore]
        public Guid id { get; set; }
    }
}
