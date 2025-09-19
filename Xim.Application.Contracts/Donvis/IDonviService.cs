using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xim.Domain.Pagings;

namespace Xim.Application.Contracts.Donvis
{
    public interface IDonviService
    {
        Task<DonviDtoView> GetAsync(Guid id);
        Task<List<DonviDtoView>> GetAsync();
        Task<DonviDtoView> CreateAsync(DonviDtoCreate model);
        Task<DonviDtoView> UpdateAsync(DonviDtoUpdate model);
        Task DeleteAsync(Guid id);
        Task<PagingData> GetListAsync(PagingParam param);
    }
}
