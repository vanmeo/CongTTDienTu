using System;
using System.Collections.Generic;
using System.Text;

namespace Xim.Library.Attributes
{
    /// <summary>
    /// Attribute đánh dấu tên bảng
    /// </summary>
    public class TableAttribute : Attribute
    {
        /// <summary>
        /// Tên bảng chi tiết
        /// </summary>
        public string Table { get; set; }

        /// <summary>
        /// Tên view cấu hình để load dữ liệu cho màn hình danh sách
        /// Sử dụng cho api list, list-summary load dữ leiuẹ paging cho màn hình danh sách
        /// Tình huống cần join với các bảng khác để lên số liệu thì mới cần
        /// Còn mặc định sẽ sử dụng tên bảng gốc <see cref="TableAttribute.Table"/>
        /// </summary>
        public string ViewList { get; set; }

        /// <summary>
        /// Khởi tạo
        /// </summary>
        public TableAttribute() { }

        /// <summary>
        /// Khởi tạo
        /// </summary>
        /// <param name="table">Tên bảng</param>
        public TableAttribute(string table)
        {
            this.Table = table;
        }

        /// <summary>
        /// Khởi tạo
        /// </summary>
        /// <param name="table">Tên bảng</param>
        /// <param name="schema">schema</param>
        public TableAttribute(string table, string schema)
        {
            this.Table = table;
        }
    }
}
