using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Xim.Domain.Entities;
using Xim.Domain.Pagings;

namespace Xim.Domain.Repos
{
    public interface IDMThuTruongRepo : IRepo<DMThuTruongEntity, Guid> 
    {
        Task<List<DMWithTenThuTruong>> GetListThuTruongAsync();
        Task<PagedResult> GetThuTruongByChuVuAsync(Guid idDMThuTruong, paging paging);
    }
}
