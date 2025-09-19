using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Xim.Application.Contracts.DMTailieu
{
    public class DMTailieuDtoUpdate : DMTailieuDtoCreate
    {
        [JsonIgnore]
        public Guid id { get; set; }
        public Guid? ModifiedBy { get; set; }
    }
}
