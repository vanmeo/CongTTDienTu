using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using Xim.Application.Contracts.DMThuTruong;

namespace Xim.Application.Contracts.DMThuTruong
{
    public class DMThuTruongDtoUpdate : DMThuTruongDtoCreate
    {
        [JsonIgnore]
        public Guid id { get; set; }
    }
}
