using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Xim.Application.Contracts.Users
{
    public class UserDtoUpdate : UserDtoCreate
    {
        [JsonIgnore]
        public Guid id { get; set; }
    }
}
