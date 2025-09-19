using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xim.Domain.Pagings;
using Xim.Domain.Repos;


namespace Xim.Application.Contracts.Menu
{
    public interface IMenuService
    {
        Task<MenuDtoView> GetAsync(Guid id);
        Task<List<MenuDtoView>> GetAsync();
        Task<MenuDtoView> CreateAsync(MenuDtoCreate model);
        Task<MenuDtoView> UpdateAsync(MenuDtoUpdate model);
        Task DeleteAsync(Guid id);
        Task<PagingData> GetListAsync(PagingParam param);
        Task<List<MenuWithSubMenu>> GetListMenuAsync();
        Task<PagedResult> GetTinBaibyMenuAsync(Guid idmenu, paging paging);
        Task<PagedResult> GetTinBaibyMenucosubAsync(Guid idmenu, paging paging);
        Task<PagedResult> GetTaiLieubyMenuAsync(Guid idmenu, paging paging);
        Task<PagedResult> GetTaiLieubyMenucosubAsync(Guid idmenu, paging paging);
    }
}
