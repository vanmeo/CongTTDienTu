using System;
using System.Collections.Generic;
using System.Text;

namespace Xim.Library.Constants
{
    /// <summary>
    /// Loại sum
    /// </summary>
    public enum SummaryType
    {
        /// <summary>
        /// Không Sum
        /// </summary>
        None = 0,
        /// <summary>
        /// Sum từ source
        /// </summary>
        Sum = 1,
        /// <summary>
        /// Trung bình
        /// </summary>
        Avg = 2,
        /// <summary>
        /// Nhỏ nhất
        /// </summary>
        Min = 3,
        /// <summary>
        /// Lớn nhất
        /// </summary>
        Max = 4,
        /// <summary>
        /// Text custom
        /// </summary>
        Text = 5,
        /// <summary>
		/// Dữ liệu sẽ trả trong data luôn
		/// </summary>
		Remote = 6,
    }
}
