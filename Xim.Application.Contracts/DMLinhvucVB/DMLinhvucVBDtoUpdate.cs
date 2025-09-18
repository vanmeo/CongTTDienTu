using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Xim.Application.Contracts.DMLinhvucVB
{
    public class DMLinhvucVBDtoUpdate : DMLinhvucVBDtoCreate
    {
        [JsonIgnore]
        public Guid id { get; set; }
    }
}
