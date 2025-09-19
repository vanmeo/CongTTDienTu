using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xim.Domain.Pagings;


namespace Xim.Application.Contracts.HoatDong
{
    public interface IHoatDongService
    {
        Task<HoatDongDtoView> GetAsync(Guid id);
        Task<List<HoatDongDtoView>> GetAsync();
        Task<HoatDongDtoView> CreateAsync(HoatDongDtoCreate model);
        Task<HoatDongDtoView> UpdateAsync(HoatDongDtoUpdate model);
        Task DeleteAsync(Guid id);
        Task<PagingData> GetListAsync(PagingParam param);
    }
}
