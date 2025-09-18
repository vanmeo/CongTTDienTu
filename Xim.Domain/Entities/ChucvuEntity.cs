using System;
using System.Collections.Generic;
using System.Text;

namespace Xim.Domain.Entities
{
    public class ChucvuEntity : BaseEntity<Guid>, IEntityCreated
    {
        public string ten { get; set; }
        public string viettat { get; set; }
        public int thutu { get; set; }
        public DateTime? created { get; set; }
    }
}
