using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xim.Domain.Pagings;


namespace Xim.Application.Contracts.CaiDat
{
    public interface ICaiDatService
    {
        Task<CaiDatDtoView> GetAsync(Guid id);
        Task<List<CaiDatDtoView>> GetAsync();
        Task<CaiDatDtoView> CreateAsync(CaiDatDtoCreate model);
        Task<CaiDatDtoView> UpdateAsync(CaiDatDtoUpdate model);
        Task DeleteAsync(Guid id);
        Task<PagingData> GetListAsync(PagingParam param);
    }
}
