using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xim.Domain.Pagings;
using Xim.Domain.Repos;


namespace Xim.Application.Contracts.QuyenHan
{
    public interface IQuyenHanService
    {
        Task<QuyenHanDtoView> GetAsync(Guid id);
        Task<List<QuyenHanDtoView>> GetAsync();
        Task<QuyenHanDtoView> CreateAsync(QuyenHanDtoCreate model);
        Task<QuyenHanDtoView> UpdateAsync(QuyenHanDtoUpdate model);
        Task DeleteAsync(Guid id);
        Task<PagingData> GetListAsync(PagingParam param);
    
    }
}
