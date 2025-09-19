using Xim.Domain.Entities;
using Xim.Domain.Repos;
using Xim.Library.Exceptions;
using Xim.Library.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xim.Domain.Pagings;
using Xim.Application.Contracts.HoatDong;
using System.Runtime.InteropServices;

namespace Xim.Application.Services
{
    public class HoatDongService : BaseService<IHoatDongRepo>, IHoatDongService
    {
        public HoatDongService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<HoatDongDtoView> GetAsync(Guid id)
        {
            var entity = await _repo.GetAsync(id);
            var data = ClassExtension.Map<HoatDongDtoView>(entity);
           
            return data;
        }

        public async Task<List<HoatDongDtoView>> GetAsync()
        {
            var entitys = await _repo.GetsAsync();
            var data = ClassExtension.Map<HoatDongDtoView>(entitys);
            return data;
        }

        public async Task<HoatDongDtoView> CreateAsync(HoatDongDtoCreate model)
        {
           
           var entity = ClassExtension.Map<HoatDongEntity>(model);
            this.ProcessInsertData(entity);
     
            entity.id = Guid.NewGuid();

            entity.is_deleted = false;
            await _repo.InsertAsync(entity);
            var result = ClassExtension.Map<HoatDongDtoView>(entity);
            return result;
        }

        public async Task<HoatDongDtoView> UpdateAsync(HoatDongDtoUpdate model)
        {
            var entity = await _repo.GetAsync(model.id);
            if (entity == null)
            {
                throw new BusinessException("Notfound");
            }
            if (entity.Url_Anh != null && model.Url_Anh is null)
            {
                model.Url_Anh = entity.Url_Anh;
            }
            ClassExtension.Map(model, entity);
            this.ProcessUpdateData(entity);
           
            await _repo.UpdateAsync(entity);

            ////log
            //await this.WriteOrgLogAsync(entity.id, OrgLogType.OrgUpdate, model);

            var result = ClassExtension.Map<HoatDongDtoView>(entity);
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
