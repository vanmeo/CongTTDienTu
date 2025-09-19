using Xim.Library.Attributes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Xim.Domain.Entities
{
    public class MenuSubEntity : BaseEntity<Guid>, IEntityCreated
    {
        public string Ma { get; set; }
        public string Ten { get; set; }
        public string GhiChu { get; set; }
        public string tenlink { get; set; }
        public Guid? parent_id { get; set; }
        public int iType { get; set; } 
        public int thutu { get; set; }
        public DateTime? created { get; set; }
    }
}
