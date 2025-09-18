using Xim.Library.Attributes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Xim.Domain.Entities
{
    public class UserEntity : BaseEntity<Guid>, IEntityCreated
    {
        public string username { get; set; }
        public string fullname { get; set; }
        public string password { get; set; }
        public DateTime? created { get; set; }
        public bool is_locked { get; set; } = false;
        public bool is_deleted { get; set; } = false;
        public Guid id_donvi { get; set; }
        public Guid? id_capbac { get; set; }
        public Guid? id_chucvu { get; set; }
      
    }
}
