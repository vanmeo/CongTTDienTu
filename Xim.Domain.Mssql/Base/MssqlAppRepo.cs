using Xim.Domain.Entities;
using System;

namespace Xim.Domain.Mssql.Base
{
    public class MssqlAppRepo<TEntity, TKey> : MssqlRepo<TEntity, TKey>
        where TEntity : IEntityKey<TKey>
    {
        public MssqlAppRepo(IServiceProvider serviceProvider) : base(MssqlFactory.AppConnection, serviceProvider)
        {
        }
    }
}
