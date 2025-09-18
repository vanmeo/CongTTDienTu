using System;
using System.Collections.Generic;
using System.Text;
using Xim.Application.Contracts.DashBoardDK;

namespace Xim.Application.Contracts.DashBoardDK
{
    public class DashBoardDKDtoView : DashBoardDKDtoCreate
    {

        public Guid? updateby { get; set; }
        public DateTime? created { get; set; }
        public DateTime? modified { get; set; }
    }
}
