using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Xim.Application.Contracts.Users
{
    public class UserDtoDelete
    {
        public Guid id { get; set; }
        [JsonIgnore]
        public Guid? created_userid { get; set; }
    }
}
