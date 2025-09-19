using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xim.Application.Contracts.Users;
using Xim.Domain.Pagings;


namespace Xim.Application.Contracts.MenuSub
{
    public interface IMenuSubService
    {
        Task<MenuSubDtoView> GetAsync(Guid id);
        Task<List<MenuSubDtoView>> GetAsync();
        Task<List<MenuSubDtoView>> GetbyMenuIdAsync(Guid idmenu);
        Task<MenuSubDtoView> CreateAsync(MenuSubDtoCreate model);
        Task<MenuSubDtoView> UpdateAsync(MenuSubDtoUpdate model);
        Task DeleteAsync(Guid id);
        Task<PagingData> GetListAsync(PagingParam param);
        Task<PagedResult> GetTinBaibySubMenuAsync(Guid idmenu, paging paging);
        Task<PagedResult> GetTaiLieubySubMenuAsync(Guid idmenu, paging paging);
    }
}
