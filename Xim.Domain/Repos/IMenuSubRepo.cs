using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Xim.Domain.Entities;
using Xim.Domain.Pagings;

namespace Xim.Domain.Repos
{
    public interface IMenuSubRepo : IRepo<MenuSubEntity, Guid> 
    {
        Task<List<T>> GetSubMenuByMenuAsync<T>(Guid menuId);
        Task<PagedResult> GetTinBaibySubMenuAsync(Guid idmenu, paging paging);
        Task<PagedResult> GetTaiLieubySubMenuAsync(Guid idmenu, paging paging);
    }
}
