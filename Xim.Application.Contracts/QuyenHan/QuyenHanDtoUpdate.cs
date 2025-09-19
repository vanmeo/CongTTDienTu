using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Xim.Application.Contracts.QuyenHan
{
    public class QuyenHanDtoUpdate : QuyenHanDtoCreate
    {
        [JsonIgnore]
        public Guid id { get; set; }
    }
}
