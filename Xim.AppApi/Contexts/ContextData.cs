using System;
using System.Collections.Generic;
using System.Text;

namespace Xim.AppApi.Contexts
{
    public class ContextData
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Language { get; set; }
        /// <summary>
        /// Login qua đâu
        /// 0 web, 1 mobile
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// Đơn vị
        /// </summary>
        public Guid OrgId { get; set; }
    }
}
