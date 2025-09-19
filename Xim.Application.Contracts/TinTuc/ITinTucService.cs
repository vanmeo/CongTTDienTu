using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xim.Domain.Pagings;


namespace Xim.Application.Contracts.TinTuc
{
    public interface ITinTucService
    {
        Task<TinTucDtoView> GetAsync(Guid id);
        Task<List<TinTucDtoView>> GetAsync();
        Task<TinTucDtoView> CreateAsync(TinTucDtoCreate model);
        Task<TinTucDtoView> UpdateAsync(TinTucDtoUpdate model);
        Task DeleteAsync(Guid id);
        Task<PagingData> GetListAsync(PagingParam param);
    }
}
