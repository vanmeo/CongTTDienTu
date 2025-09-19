using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xim.Domain.Pagings;

namespace Xim.Application.Contracts.Chucvus
{
    public interface IChucvuService
    {
        Task<ChucvuDtoView> GetAsync(Guid id);
        Task<List<ChucvuDtoView>> GetAsync();
        Task<ChucvuDtoView> CreateAsync(ChucvuDtoCreate model);
        Task<ChucvuDtoView> UpdateAsync(ChucvuDtoUpdate model);
        Task DeleteAsync(Guid id);
        Task<PagingData> GetListAsync(PagingParam param);
    }
}
