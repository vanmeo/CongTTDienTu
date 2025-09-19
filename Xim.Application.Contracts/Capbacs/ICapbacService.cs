using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xim.Domain.Pagings;


namespace Xim.Application.Contracts.Capbacs
{
    public interface ICapbacService
    {
        Task<CapbacDtoView> GetAsync(Guid id);
        Task<List<CapbacDtoView>> GetAsync();
        Task<CapbacDtoView> CreateAsync(CapbacDtoCreate model);
        Task<CapbacDtoView> UpdateAsync(CapbacDtoUpdate model);
        Task DeleteAsync(Guid id);
        Task<PagingData> GetListAsync(PagingParam param);
    }
}
