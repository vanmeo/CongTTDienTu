using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xim.Domain.Pagings;


namespace Xim.Application.Contracts.ThuTruongBQP
{
    public interface IThuTruongBQPService
    {
        Task<ThuTruongBQPDtoView> GetAsync(Guid id);
        Task<List<ThuTruongBQPDtoView>> GetAsync();
        Task<ThuTruongBQPDtoView> CreateAsync(ThuTruongBQPDtoCreate model);
        Task<ThuTruongBQPDtoView> UpdateAsync(ThuTruongBQPDtoUpdate model);
        Task DeleteAsync(Guid id);
        Task<PagingData> GetListAsync(PagingParam param);
    }
}
