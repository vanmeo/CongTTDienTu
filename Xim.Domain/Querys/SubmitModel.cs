using System;
using System.Collections.Generic;
using System.Text;
using Xim.Library.Constants;

namespace Xim.Domain.Querys
{
    /// <summary>
    /// Đối tượng submit dữ liệu vào db
    /// </summary>
    public class SubmitModel
    {
        /// <summary>
        /// Bảng dữ liệu nào
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// Trạng thái bản ghi là thêm/sửa/xóa
        /// </summary>
        public ModelState State { get; set; }
        /// <summary>
        /// Dữ liệu
        /// </summary>
        public List<Dictionary<string, object>> Datas { get; set; }
        /// <summary>
        /// Tên trường khóa chính
        /// </summary>
        public List<string> KeyFields { get; set; }
    }
}
