using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xim.Domain.Pagings;

namespace Xim.Application.Contracts.Users
{
    public interface IUserService
    {
        Task<UserDtoView> GetAsync(Guid id);
        Task<List<UserDtoView>> GetAsync();
        Task<List<UserDtoView>> GetByDonviAsync(Guid iddonvi);
        Task<UserDtoLoggedIn> GetLoginAsync(UserDtoLogin model);
        Task<UserDtoView> CreateAsync(UserDtoCreate model);
        Task<UserDtoView> UpdateAsync(UserDtoUpdate model);
        Task ChangePasswordAsync(UserDtoChangePassword model);
        Task DeleteAsync(Guid id);
        Task<PagingData> GetListAsync(PagingParam param);
    }
}
