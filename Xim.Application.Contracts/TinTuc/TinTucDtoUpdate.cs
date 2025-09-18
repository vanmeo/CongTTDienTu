using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Xim.Application.Contracts.TinTuc
{
    public class TinTucDtoUpdate : TinTucDtoCreate
    {
        [JsonIgnore]
        public Guid id { get; set; }
        public Guid? updateby { get; set; }
    }
}
