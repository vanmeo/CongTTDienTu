using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xim.Domain.Pagings;


namespace Xim.Application.Contracts.DMCoQuanBHVB
{
    public interface IDMCoQuanBHVBService
    {
        Task<DMCoQuanBHVBDtoView> GetAsync(Guid id);
        Task<List<DMCoQuanBHVBDtoView>> GetAsync();
        Task<DMCoQuanBHVBDtoView> CreateAsync(DMCoQuanBHVBDtoCreate model);
        Task<DMCoQuanBHVBDtoView> UpdateAsync(DMCoQuanBHVBDtoUpdate model);
        Task DeleteAsync(Guid id);
        Task<PagingData> GetListAsync(PagingParam param);
    }
}
