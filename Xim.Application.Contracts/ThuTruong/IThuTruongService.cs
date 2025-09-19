using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xim.Domain.Pagings;


namespace Xim.Application.Contracts.ThuTruong
{
    public interface IThuTruongService
    {
        Task<ThuTruongDtoView> GetAsync(Guid id);
        Task<List<ThuTruongDtoView>> GetAsync();
        Task<ThuTruongDtoView> CreateAsync(ThuTruongDtoCreate model);
        Task<ThuTruongDtoView> UpdateAsync(ThuTruongDtoUpdate model);
        Task DeleteAsync(Guid id);
        Task<PagingData> GetListAsync(PagingParam param);
    }
}
