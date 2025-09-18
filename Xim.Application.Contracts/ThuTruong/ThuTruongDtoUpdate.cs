using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using Xim.Application.Contracts.TinTuc;

namespace Xim.Application.Contracts.ThuTruong
{
    public class ThuTruongDtoUpdate : ThuTruongDtoCreate
    {
        [JsonIgnore]
        public Guid id { get; set; }
        public Guid? updateby { get; set; }
    }
}
