using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Xim.Application.Contracts.DashBoardDK
{
    public class DashBoardDKDtoUpdate : DashBoardDKDtoCreate
    {
        [JsonIgnore]
        public Guid id { get; set; }
        public Guid? updateby { get; set; }
    }
}
