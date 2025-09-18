using Xim.Library.Attributes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Xim.Domain.Entities
{
    public class DMThuTruongEntity : BaseEntity<Guid>, IEntityCreated
    {
        public string Ma { get; set; }
        public string TenChucVu { get; set; }
        public string GhiChu { get; set; }
        public int thutu { get; set; }
        public DateTime? created { get; set; }
    }
}
