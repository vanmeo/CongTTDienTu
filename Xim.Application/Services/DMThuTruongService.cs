using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xim.Application.Contracts.DMThuTruong;
using Xim.Application.Contracts.Donvis;
using Xim.Application.Contracts.Menu;
using Xim.Domain.Entities;
using Xim.Domain.Pagings;
using Xim.Domain.Repos;
using Xim.Library.Constants;
using Xim.Library.Exceptions;
using Xim.Library.Extensions;

namespace Xim.Application.Services
{
    public class DMThuTruongService : BaseService<IDMThuTruongRepo>, IDMThuTruongService

    {
        public DMThuTruongService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<DMThuTruongDtoView> GetAsync(Guid id)
        {
            var entity = await _repo.GetAsync(id);
            var data = ClassExtension.Map<DMThuTruongDtoView>(entity);
            return data;
        }
        public async Task<List<DMThuTruongDtoView>> GetAsync()
        {
            var entitys = await _repo.GetsAsync();
            var data = ClassExtension.Map<DMThuTruongDtoView>(entitys);
            return data;
        }
        public async Task<DMThuTruongDtoView> CreateAsync(DMThuTruongDtoCreate model)
        {
            var entity = await _repo.GetAsync<DMThuTruongEntity>(new Dictionary<string, object>
            {
                { "TenChucVu", model.TenChucVu },
            });
            if (entity != null)
            {
                throw new BusinessException($"Đã tồn tại Danh mục thủ trưởng này");
            }
            entity = ClassExtension.Map<DMThuTruongEntity>(model);

            entity.Ma = this.ProcessMa(entity.TenChucVu);
            this.ProcessInsertData(entity);
            entity.id = Guid.NewGuid();
            await _repo.InsertAsync(entity);
            var result = ClassExtension.Map<DMThuTruongDtoView>(entity);
            return result;
        }

        public async Task<DMThuTruongDtoView> UpdateAsync(DMThuTruongDtoUpdate model)
        {
            var entity = await _repo.GetAsync(model.id);
            if (entity == null)
            {
                throw new BusinessException("Notfound");
            }
          
            ClassExtension.Map(model, entity);
            this.ProcessUpdateData(entity);
          
            entity.Ma = this.ProcessMa(entity.TenChucVu);
            
            await _repo.UpdateAsync(entity);

            ////log
            //await this.WriteOrgLogAsync(entity.id, OrgLogType.OrgUpdate, model);

            var result = ClassExtension.Map<DMThuTruongDtoView>(entity);
            return result;
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _repo.GetAsync(id);
            if (entity == null)
            {
                throw new BusinessException("Notfound");
            }

            this.ProcessUpdateData(entity);
            await _repo.DeleteAsync(id);
          
        }

        public async Task<PagingData> GetListAsync(PagingParam param)
        {
            var entity = await _repo.GetPagingAsync(param);
            var sum = await _repo.GetPagingSummaryAsync(new PagingSummaryParam()
            {
                filter = param.filter
            });
            return new PagingData
            {
                data = entity,
                sumData = sum
            };
        }



        public async Task<List<DMWithTenThuTruong>> GetListThuTruongAsync()
        {
            var data = await _repo.GetListThuTruongAsync();
            return data;
        }
        public async Task<PagedResult> GetThuTruongByChuVuAsync(Guid idDMThuTruong, paging paging)
        {
            var data = await _repo.GetThuTruongByChuVuAsync(idDMThuTruong, paging);
            return data;
        }

    }
}
