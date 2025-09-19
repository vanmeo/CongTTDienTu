using System;
using System.Collections.Generic;
using System.Text;

namespace Xim.Domain.Mssql.Tables
{
    public class TableConfig
    {
        public string TableName { get; set; }
        public string KeyField { get; set; }
        public string ViewPaging { get; set; }

        public TableConfig(string tableName, string keyField = "id", string viewPaging = null)
        {
            TableName = tableName;
            KeyField = keyField;
            ViewPaging = viewPaging;
        }
    }
}
