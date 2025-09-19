using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xim.Domain.Pagings;
using Xim.Domain.Repos;


namespace Xim.Application.Contracts.DMThuTruong
{
    public interface IDMThuTruongService
    {
        Task<DMThuTruongDtoView> GetAsync(Guid id);
        Task<List<DMThuTruongDtoView>> GetAsync();
        Task<DMThuTruongDtoView> CreateAsync(DMThuTruongDtoCreate model);
        Task<DMThuTruongDtoView> UpdateAsync(DMThuTruongDtoUpdate model);
        Task DeleteAsync(Guid id);
        Task<PagingData> GetListAsync(PagingParam param);
        Task<List<DMWithTenThuTruong>> GetListThuTruongAsync();
        Task<PagedResult> GetThuTruongByChuVuAsync(Guid idDMThuTruong, paging paging);

   

    }
}
