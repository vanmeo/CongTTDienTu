using Xim.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Xim.Domain.Repos
{
    public class AlbumWithAnh
    {
        public AlbumEntity album { get; set; }
        public List<Anh_AlbumEntity> DanhsachAnh_Album { get; set; }
    }
}
