using Xim.Domain.Entities;
using Xim.Domain.Mssql.Base;
using Xim.Domain.Repos;
using System;

namespace Xim.Domain.Mssql.Repos
{
    internal class QuyenHanRepo : MssqlAppRepo<QuyenHanEntity, Guid>, IQuyenHanRepo
    {
        public QuyenHanRepo(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}
