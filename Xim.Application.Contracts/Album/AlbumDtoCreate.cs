using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Xim.Application.Contracts.Album

{
    public class AlbumDtoCreate
    {
        public int thutu { get; set; } = 1;
        public string Tenalbum { get; set; }
        public bool? is_video { get; set; }
        //public string Link { get; set; }
        //public string Icon { get; set; }

    }
}