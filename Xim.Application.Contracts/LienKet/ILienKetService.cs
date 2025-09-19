using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xim.Domain.Pagings;


namespace Xim.Application.Contracts.LienKet
{
    public interface ILienKetService
    {
        Task<LienKetDtoView> GetAsync(Guid id);
        Task<List<LienKetDtoView>> GetAsync();
        Task<LienKetDtoView> CreateAsync(LienKetDtoCreate model);
        Task<LienKetDtoView> UpdateAsync(LienKetDtoUpdate model);
        Task DeleteAsync(Guid id);
        Task<PagingData> GetListAsync(PagingParam param);
    }
}
