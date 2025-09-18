using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xim.Domain.Pagings;
using Xim.Domain.Repos;


namespace Xim.Application.Contracts.Album
{
    public interface IAlbumService
    {
        Task<AlbumDtoView> GetAsync(Guid id);
        Task<List<AlbumDtoView>> GetAsync();
        Task<AlbumDtoView> CreateAsync(AlbumDtoCreate model);
        Task<AlbumDtoView> UpdateAsync(AlbumDtoUpdate model);
        Task DeleteAsync(Guid id);
        Task<PagingData> GetListAsync(PagingParam param);
        Task<List<AlbumWithAnh>> GetListAlbumAsync();
        Task<PagedResult> GetListAnhByAlbum(Guid idAlbum, paging paging);
    }
}
