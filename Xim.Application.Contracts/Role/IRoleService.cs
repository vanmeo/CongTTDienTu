using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xim.Domain.Entities;
using Xim.Domain.Pagings;

namespace Xim.Application.Contracts.Role

{
    public interface IRoleService
    {
        Task<PagingData> GetListAsync(PagingParam param);
        Task<RoleEntity> CreateAsync(RoleDtoCreate model);
        Task DeleteAsync(Guid id);
        Task<RoleDtoView> GetAsync(Guid id);
        Task<RoleDtoView> UpdateAsync(RoleDtoUpdate model);
        Task<List<UserRoleEntity>> AddRoleForUser(UserRoleDtoCreate model);
        Task<List<RolePermissionEntity>> GetRolePermissionByRole(Guid role_id);
        Task<List<RolePermissionEntity>> AddRolePermission(List<RolePermisstionDtoCreate> lstModel);
    }
}
