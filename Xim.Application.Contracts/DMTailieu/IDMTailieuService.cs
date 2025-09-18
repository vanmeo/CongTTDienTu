using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xim.Domain.Pagings;


namespace Xim.Application.Contracts.DMTailieu
{
    public interface IDMTailieuService
    {
        Task<DMTailieuDtoView> GetAsync(Guid id);
        Task<List<DMTailieuDtoView>> GetAsync();
        Task<DMTailieuDtoView> CreateAsync(DMTailieuDtoCreate model);
        Task<DMTailieuDtoView> UpdateAsync(DMTailieuDtoUpdate model);
        Task DeleteAsync(Guid id);
        Task<PagingData> GetListAsync(PagingParam param);
    }
}
