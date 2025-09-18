using System;
using System.Collections.Generic;
using System.Text;

namespace Xim.Storage
{
    public class StorageConfig
    {
        public List<string> UploadAllowExtensions { get; set; }
        public int? UploadMaxSizeMB { get; set; }

        public Dictionary<string, string> ContentType { get; set; }
        public string ContentTypeDefault { get; set; }

        /// <summary>
        /// Thời gian output cache của ảnh
        /// </summary>
        public int ImageCacheSecond { get; set; }
        /// <summary>
        /// Thời gian cache các file js/css
        /// </summary>
        public int StaticResourceCacheSecond { get; set; }
    }
}
