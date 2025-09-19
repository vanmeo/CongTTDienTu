using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xim.Domain.Pagings;


namespace Xim.Application.Contracts.DMLinhvucVB
{
    public interface IDMLinhvucVBService
    {
        Task<DMLinhvucVBDtoView> GetAsync(Guid id);
        Task<List<DMLinhvucVBDtoView>> GetAsync();
        Task<DMLinhvucVBDtoView> CreateAsync(DMLinhvucVBDtoCreate model);
        Task<DMLinhvucVBDtoView> UpdateAsync(DMLinhvucVBDtoUpdate model);
        Task DeleteAsync(Guid id);
        Task<PagingData> GetListAsync(PagingParam param);
    }
}
