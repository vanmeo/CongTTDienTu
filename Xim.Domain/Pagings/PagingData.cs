using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Xim.Domain.Pagings
{
    public class PagingData
    {
        public IList data { get; set; }
        public object sumData { get; set; }        
    }
}
