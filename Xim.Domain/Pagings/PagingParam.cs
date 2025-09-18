using System;
using System.Collections.Generic;
using System.Text;

namespace Xim.Domain.Pagings
{
    public class PagingParam
    {
        public string sort { get; set; }
        public int skip { get; set; }
        public int take { get; set; }
        public string filter { get; set; }
        public string columns { get; set; }
    }
}
