using System;

namespace Xim.Application.Contracts.Users
{
    public class UserDtoLogin
    {
        public string username { get; set; }
        public string password { get; set; }
    }

    public class UserDtoLoggedIn
    {
        public Guid id { get; set; }
        public string username { get; set; }
        public string fullname { get; set; }
        public DateTime? created { get; set; }
    }
}
