using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Xim.Application.Contracts.Users
{
    public class UserDtoCreate
    {
        public string username { get; set; }
        public string fullname { get; set; }
        public string password { get; set; }
        public Guid? id_donvi { get; set; }
        public Guid? id_capbac { get; set; }
        public Guid? id_chucvu { get; set; }
    }
}
