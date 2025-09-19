using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xim.Domain.Mssql.Providers
{
    internal struct MssqlDbTypeMapEntry
    {
        public Type Type;
        public DbType DbType;
        public SqlDbType SqlDbType;
        public MssqlDbTypeMapEntry(Type type, DbType dbType, SqlDbType sqlDbType)
        {
            this.Type = type;
            this.DbType = dbType;
            this.SqlDbType = sqlDbType;
        }
    }
}
