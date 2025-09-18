using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Xim.Application.Contracts.Users
{
    public class UserDtoChangePassword
    {
        [JsonIgnore]
        public Guid? userid { get; set; }
        public string old_password { get; set; }
        public string new_password { get; set; }
    }
}
