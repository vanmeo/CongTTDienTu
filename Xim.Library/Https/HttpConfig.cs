using System;
using System.Collections.Generic;
using System.Text;

namespace Xim.Library.Https
{
    /// <summary>
    /// Cấu hình sử dụng http
    /// </summary>
    public class HttpConfig
    {
        public string Url { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public int? Timeout { get; set; }
    }
}
