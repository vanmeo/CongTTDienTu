using Xim.Library.Attributes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Xim.Domain.Entities
{
    public class DonviEntity : BaseEntity<Guid>, IEntityCreated
    {
        public string ten { get; set; }
        public string viettat { get; set; }
        public int thutu { get; set; }
        public Guid id_parent { get; set; }
        public DateTime? created { get; set; }
    }
}
