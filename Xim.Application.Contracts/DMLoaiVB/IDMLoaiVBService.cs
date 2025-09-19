using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xim.Domain.Pagings;


namespace Xim.Application.Contracts.DMLoaiVB
{
    public interface IDMLoaiVBService
    {
        Task<DMLoaiVBDtoView> GetAsync(Guid id);
        Task<List<DMLoaiVBDtoView>> GetAsync();
        Task<DMLoaiVBDtoView> CreateAsync(DMLoaiVBDtoCreate model);
        Task<DMLoaiVBDtoView> UpdateAsync(DMLoaiVBDtoUpdate model);
        Task DeleteAsync(Guid id);
        Task<PagingData> GetListAsync(PagingParam param);
    }
}
