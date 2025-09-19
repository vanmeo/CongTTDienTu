using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xim.Domain.Pagings;
using Xim.Domain.Repos;


namespace Xim.Application.Contracts.BienNienSuKien
{
    public interface IBienNienSuKienService
    {
        Task<BienNienSuKienDtoView> GetAsync(Guid id);
        Task<List<BienNienSuKienDtoView>> GetAsync();
        Task<BienNienSuKienDtoView> CreateAsync(BienNienSuKienDtoCreate model);
        Task<BienNienSuKienDtoView> UpdateAsync(BienNienSuKienDtoUpdate model);
        Task DeleteAsync(Guid id);
        Task<PagingData> GetListAsync(PagingParam param);

    }
}
