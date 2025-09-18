using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xim.Domain.Entities;
using Xim.Domain.Pagings;


namespace Xim.Application.Contracts.DashBoardDK
{
    public interface IDashBoardDKService
    {
        Task<DashBoardDKDtoView> GetAsync(Guid id);
        Task<List<DashBoardDKDtoView>> GetAsync();
       Task<DashBoardDKDtoView> GetDashboard();
        Task<DashBoardDKDtoView> CreateAsync(DashBoardDKDtoCreate model);
        Task<DashBoardDKDtoView> UpdateAsync(DashBoardDKDtoUpdate model);
        Task DeleteAsync(Guid id);
        Task<PagingData> GetListAsync(PagingParam param);
    }
}
