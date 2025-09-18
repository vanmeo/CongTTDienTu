using Xim.Domain.Entities;
using Xim.Domain.Mssql.Base;
using Xim.Domain.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Xim.Application.Contracts.Donvis;
using Xim.Library.Exceptions;
using Xim.Library.Extensions;

namespace Xim.Domain.Mssql.Repos
{
    internal class RoleRepo : MssqlAppRepo<RoleEntity, Guid>, IRoleRepo
    {
        public RoleRepo(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }

    }
}
