using Xim.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Xim.Domain.Repos
{
    public interface IRoleRepo : IRepo<RoleEntity, Guid>
    {
       //public Task AddRoleForUser(List <Guid> L_idUser, Guid id_role);
    }
}
