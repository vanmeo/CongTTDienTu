using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Xim.Domain.Entities;
using Xim.Domain.Pagings;

namespace Xim.Domain.Repos
{
    public interface IMenuRepo : IRepo<MenuEntity, Guid> 
    {
        Task<List<MenuWithSubMenu>> GetListMenuAsync();
        Task<PagedResult> GetTinBaibyMenuAsync(Guid idmenu, paging paging);
        Task<PagedResult> GetTinBaibyMenucosubAsync(Guid idmenu, paging paging);
        Task<PagedResult> GetTaiLieubyMenuAsync(Guid idmenu, paging paging);
        Task<PagedResult> GetTaiLieubyMenucosubAsync(Guid idmenu, paging paging);
    }
}
