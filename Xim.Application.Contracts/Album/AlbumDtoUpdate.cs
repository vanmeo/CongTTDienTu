using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Xim.Application.Contracts.Album
{
    public class AlbumDtoUpdate : AlbumDtoCreate
    {
        [JsonIgnore]
        public Guid id { get; set; }
      
      

    }
}
