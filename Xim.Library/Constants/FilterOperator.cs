using System;
using System.Collections.Generic;
using System.Text;

namespace Xim.Library.Constants
{
    /// <summary>
    /// Các toán tử phục vụ filter
    /// </summary>
    public static class FilterOperator
    {
        public const string Contains = "*";
        public const string Notcontains = "!*";
        public const string StartsWith = "*.";
        public const string EndsWith = ".*";
        public const string Null = "NULL";
        public const string NotNull = "NOT NULL";
        public const string Equals = "=";
        public const string NotEquals = "!=";
        public const string GreaterThan = ">";
        public const string GreaterThanEquals = ">=";
        public const string LessThan = "<";
        public const string LessThanEquals = "<=";
        public const string Between = "[]";
        public const string In = "IN";
        public const string NotIn = "NOT IN";
        public const string NotStartsWith = "!*."; //Không bắt đầu bằng
        public const string NotEndsWith = "!.*";
    }
}
