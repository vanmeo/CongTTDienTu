using System;
using System.Collections.Generic;
using System.Text;

namespace Xim.Domain.Entities
{
    public class DMLoaiVBEntity : BaseEntity<Guid>, IEntityCreated
    {
        public string? ten { get; set; }
        public string? Description { get; set; }
        public string? Icon { get; set; }

        public int thutu { get; set; } = 0;
        public DateTime? created { get; set; }

    }
}
