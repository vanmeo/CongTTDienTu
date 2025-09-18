using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Xim.Domain.Mssql.Models
{
    /// <summary>
    /// Đối tượng filter truyền lên từ client
    /// </summary>
    public class MssqlFilterItem
    {
        /// <summary>
        /// Tên trường
        /// </summary>
        [JsonProperty("f")]
        public string Field { get; set; }
        /// <summary>
        /// Toán tử
        /// </summary>
        [JsonProperty("o")]
        public string Operator { get; set; }
        /// <summary>
        /// Giá trị
        /// </summary>
        [JsonProperty("v")]
        public object Value { get; set; }
        /// <summary>
        /// Or với item filter khác
        /// </summary>
        [JsonProperty("os")]
        public List<MssqlFilterItem> Ors { get; set; }
    }
}
