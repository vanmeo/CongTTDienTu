using System;
using System.Collections.Generic;
using System.Text;

namespace Xim.AppApi.Contexts
{
    /// <summary>
    /// Tương tác với context
    /// </summary>
    public interface IContextService
    {
        /// <summary>
        /// Đọc context
        /// </summary>
        ContextData Get();
    }
}
