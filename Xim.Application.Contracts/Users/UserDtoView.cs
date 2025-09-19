using System;
using System.Collections.Generic;
using System.Text;

namespace Xim.Application.Contracts.Users
{
    public class UserDtoView
    {
        public string username { get; set; }
        public string fullname { get; set; }
        public DateTime? created { get; set; }
        public bool? is_blocked { get; set; }
        public Guid? id_donvi { get; set; }
        public Guid? id_capbac { get; set; }
        public Guid? id_chucvu { get; set; }
    }
}
