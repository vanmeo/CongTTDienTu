using Xim.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Xim.Domain.Repos
{
    public interface IUserRepo : IRepo<UserEntity, Guid>
    {
        Task<UserEntity> GetLoginAsync(string username);
        Task<List<T>> GetUserByDonViAsync<T>(Guid donviId);
    }
}
