using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Xim.Domain.Entities;
using Xim.Domain.Pagings;

namespace Xim.Domain.Repos
{
    public interface IAlbumRepo : IRepo<AlbumEntity, Guid> 
    {
        Task<List<AlbumWithAnh>> GetListAlbumAsync();
        Task<PagedResult> GetListAnhByAlbum(Guid idAlbum, paging paging);
    }
}
