using System;
using System.Collections.Generic;
using System.Text;

namespace Xim.Application.Contracts.Anh_Album
{
    public class Anh_AlbumDtoView 
    {
        public Guid idalbum { get; set; }
        public int thutu { get; set; }
        public string TenAnh { get; set; }
        public string LinkAnh { get; set; }
        public string? GhiChu { get; set; }
       
    }
}
