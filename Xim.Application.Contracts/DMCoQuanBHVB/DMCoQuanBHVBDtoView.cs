using System;
using System.Collections.Generic;
using System.Text;

namespace Xim.Application.Contracts.DMCoQuanBHVB
{
    public class DMCoQuanBHVBDtoView
    {
        public Guid? id_parent { get; set; }
        public string? ten { get; set; }
        public string? description { get; set; }
        public string? icon { get; set; }
        public int thutu { get; set; } = 0;
    }
}
