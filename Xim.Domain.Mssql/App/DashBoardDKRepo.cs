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
    internal class DashBoardDKRepo : MssqlAppRepo<DashBoardDKEntity, Guid>, IDashBoardDKRepo
    {
        public DashBoardDKRepo(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
        public async Task<DashBoardDKEntity> GetDashboard()
        {

            var query = @"
            SELECT TOP 1 *
            FROM DashboardDK
            WHERE is_deleted = 0 OR is_deleted IS NULL
            ORDER BY created DESC
        ";

            var result = await Provider.QueryAsync<DashBoardDKEntity>(query, new Dictionary<string, object>());

            return result.FirstOrDefault();

        }
    }
}
