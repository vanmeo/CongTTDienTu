using Xim.Application.Contracts.Donvis;
using Xim.Domain.Entities;
using Xim.Domain.Repos;
using Xim.Library.Exceptions;
using Xim.Library.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xim.Domain.Pagings;
using Xim.Application.Contracts.BienNienSuKien;
using System.IO;

namespace Xim.Application.Services
{
    public class BienNienSuKienService : BaseService<IBienNienSuKienRepo>, IBienNienSuKienService
    {
        public BienNienSuKienService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<BienNienSuKienDtoView> GetAsync(Guid id)
        {
            var entity = await _repo.GetAsync(id);
            var data = ClassExtension.Map<BienNienSuKienDtoView>(entity);
            return data;
        }

        public async Task<List<BienNienSuKienDtoView>> GetAsync()
        {
            var entitys = await _repo.GetsAsync();
            var data = ClassExtension.Map<BienNienSuKienDtoView>(entitys);
            return data;
        }

        public async Task<BienNienSuKienDtoView> CreateAsync(BienNienSuKienDtoCreate model)
        {
            var entity = await _repo.GetAsync<BienNienSuKienEntity>(new Dictionary<string, object>
            {
                { "MocthoiGian", model.MocthoiGian },
            });
            if (entity != null)
            {
                throw new BusinessException($"Đã tồn tại Mốc thời gian này");
            }

            entity = ClassExtension.Map<BienNienSuKienEntity>(model);
            this.ProcessInsertData(entity);
            //if (entity.TinhTrang == 1)
            //    entity.NgayPH = DateTime.Now;
            entity.id = Guid.NewGuid();
            //entity.TenLink = this.ProcessTenLink(entity.HoVaTen);
            entity.ViewCount = 0;
            entity.is_deleted = false;
            await _repo.InsertAsync(entity);
            var result = ClassExtension.Map<BienNienSuKienDtoView>(entity);
            return result;
        }

        public async Task<BienNienSuKienDtoView> UpdateAsync(BienNienSuKienDtoUpdate model)
        {
            var entity = await _repo.GetAsync(model.id);
            if (entity == null)
            {
                throw new BusinessException("Notfound");
            }
            if (entity.AnhChinh != null && model.AnhChinh is null)
                model.AnhChinh = entity.AnhChinh;

            ClassExtension.Map(model, entity);
            this.ProcessUpdateData(entity);
            //if (entity.TinhTrang == 1&&entity.NgayPH!=null)
            //    entity.NgayPH = DateTime.Now;
            //entity.TenLink = this.ProcessTenLink(entity.HoVaTen);
            entity.is_deleted = false;
            await _repo.UpdateAsync(entity);

            ////log
            //await this.WriteOrgLogAsync(entity.id, OrgLogType.OrgUpdate, model);

            var result = ClassExtension.Map<BienNienSuKienDtoView>(entity);
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
            await _repo.Deletebyis_deleteAsync(id);

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
    }
}
