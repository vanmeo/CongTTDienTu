using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Xim.Domain.Pagings
{
    public class PagedResult
    {
        public IList Data { get; set; }
        public int PageSize { get; set; }
        public int TotalDocuments { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
    }
}
