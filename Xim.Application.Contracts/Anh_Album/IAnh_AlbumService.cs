using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xim.Domain.Pagings;


namespace Xim.Application.Contracts.Anh_Album
{
    public interface IAnh_AlbumService
    {
        Task<Anh_AlbumDtoView> GetAsync(Guid id);
        Task<List<Anh_AlbumDtoView>> GetAsync();
        Task<Anh_AlbumDtoView> CreateAsync(Anh_AlbumDtoCreate model);
        Task<List<Anh_AlbumDtoView>> CreateAdrangeAsync(List<Anh_AlbumDtoCreate> Listmodel);
        Task<Anh_AlbumDtoView> UpdateAsync(Anh_AlbumDtoUpdate model);
        Task DeleteAsync(Guid id);
        Task<PagingData> GetListAsync(PagingParam param);
    }
}
