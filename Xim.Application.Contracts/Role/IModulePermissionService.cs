using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xim.Domain.Entities;
using Xim.Domain.Pagings;

namespace Xim.Application.Contracts.Role

{
    public interface IModulePermissionService
    {
        Task<List<ModulePermissionEntity>> GetListModulePermission();
    }
}
