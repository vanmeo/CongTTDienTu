using Xim.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Xim.Domain.Repos
{
    public class DMWithTenThuTruong
    {
        public DMThuTruongEntity DMThuTruong { get; set; }
        public List<ThuTruongBQPEntity> DanhsachThutruong { get; set; }
    }
}
