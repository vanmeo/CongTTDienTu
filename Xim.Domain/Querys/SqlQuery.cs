using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Xim.Domain.Querys
{
    /// <summary>
    /// Thông tin câu lệnh gọi vào sql
    /// </summary>
    public class SqlQuery
    {
        /// <summary>
        /// Số lượng bản ghi
        /// </summary>
        public int RecordCount { get; set; }
        /// <summary>
        /// Nội dung câu lệnh
        /// </summary>
        public string Query { get; set; }
        /// <summary>
        /// Tham số
        /// </summary>
        public Dictionary<string, object> Param { get; set; }
        /// <summary>
        /// Danh sách bản ghi tạo ra câu lệnh
        /// </summary>
        public IList Records { get; set; }
    }
}
