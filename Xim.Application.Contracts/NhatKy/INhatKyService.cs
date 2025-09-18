using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xim.Domain.Pagings;


namespace Xim.Application.Contracts.NhatKy
{
    public interface INhatKyService
    {
        Task<NhatKyDtoView> GetAsync(Guid id);
        Task<List<NhatKyDtoView>> GetAsync();
        Task<NhatKyDtoView> CreateAsync(NhatKyDtoCreate model);
        Task<NhatKyDtoView> UpdateAsync(NhatKyDtoUpdate model);
        Task DeleteAsync(Guid id);
        Task<PagingData> GetListAsync(PagingParam param);
    }
}
