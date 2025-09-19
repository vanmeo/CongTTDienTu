using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xim.Domain.Pagings;
using Xim.Domain.Repos;


namespace Xim.Application.Contracts.Module
{
    public interface IModuleService
    {
        Task<ModuleDtoView> GetAsync(Guid id);
        Task<List<ModuleDtoView>> GetAsync();
        Task<ModuleDtoView> CreateAsync(ModuleDtoCreate model);
        Task<ModuleDtoView> UpdateAsync(ModuleDtoUpdate model);
        Task DeleteAsync(Guid id);
        Task<PagingData> GetListAsync(PagingParam param);
    
    }
}
