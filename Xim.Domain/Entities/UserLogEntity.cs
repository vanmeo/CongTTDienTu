using System;
using System.Collections.Generic;
using System.Text;

namespace Xim.Domain.Entities
{
    public class UserLogEntity : BaseEntity<Guid>, IEntityCreated
    {
        public Guid user_id { get; set; }
        public string type { get; set; }
        public string data { get; set; }
        public DateTime? created { get; set; }
    }
}
