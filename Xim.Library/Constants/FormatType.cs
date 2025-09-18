using System;
using System.Collections.Generic;
using System.Text;

namespace Xim.Library.Constants
{
    /// <summary>
    /// Định dạng để hiển thị
    /// </summary>
    public enum FormatType
    {
        /// <summary>
		/// Loại mà không xử lý gì cả
		/// </summary>
		None = 0,
        /// <summary>
        /// Đơn giá
        /// </summary>
        UnitPrice = 1,
        /// <summary>
        /// Thành tiền quy đổi
        /// </summary>
        Amount = 2,
        /// <summary>
        /// Hệ số, Tỷ lệ
        /// </summary>
        Rate = 3,
        /// <summary>
        /// Tỷ giá
        /// </summary>
        ExchangeRate = 4,
        /// <summary>
        /// Số lượng
        /// </summary>
        Quantity = 11,
        /// <summary>
        /// Kiểu chữ
        /// </summary>
        Text = 12,
        /// <summary>
        /// Kiểu tích chọn
        /// </summary>
        Checkbox = 13,
        /// <summary>
        /// Kiểu ngày tháng
        /// </summary>
        Date = 14,
        /// <summary>
        /// Kiểu thời gian
        /// </summary>
        Time = 16,
        /// <summary>
        /// Kiểu ngày/tháng/năm có giờ
        /// </summary>
        DateTime = 19,
        /// <summary>
        /// Kiểu số nguyên
        /// </summary>
        Number = 20,
        /// <summary>
        /// Định dạng dữ liệu html
        /// </summary>
        Html = 99,
        /// <summary>
        /// Kiểu enum
        /// </summary>
        Enum = 100,
        /// <summary>
        /// STT
        /// </summary>
        STT = 888
    }
}
