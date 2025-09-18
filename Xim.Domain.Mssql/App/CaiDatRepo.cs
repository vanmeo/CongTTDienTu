using Xim.Domain.Entities;
using Xim.Domain.Mssql.Base;
using Xim.Domain.Mssql.Tables;
using Xim.Domain.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xim.Domain.Mssql.Repos
{
    internal class CaiDatRepo : MssqlAppRepo<CaiDatEntity, Guid>, ICaiDatRepo
    {
        public CaiDatRepo(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}
