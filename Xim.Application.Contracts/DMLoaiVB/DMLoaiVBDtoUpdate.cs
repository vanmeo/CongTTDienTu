using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Xim.Application.Contracts.DMLoaiVB
{
    public class DMLoaiVBDtoUpdate : DMLoaiVBDtoCreate
    {
        [JsonIgnore]
        public Guid id { get; set; }
    }
}
