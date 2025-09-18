using Xim.Application.Contracts.Donvis;
using Xim.Domain.Entities;
using Xim.Domain.Repos;
using Xim.Library.Exceptions;
using Xim.Library.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xim.Library.Constants;
using Xim.Domain.Pagings;
using Microsoft.Extensions.DependencyInjection;
using Xim.Application.Contracts.CaiDat;

namespace Xim.Application.Services
{
    public class CaiDatService : BaseService<ICaiDatRepo>, ICaiDatService
    {
        public CaiDatService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<CaiDatDtoView> GetAsync(Guid id)
        {
            var entity = await _repo.GetAsync(id);
            var data = ClassExtension.Map<CaiDatDtoView>(entity);
            return data;
        }

        public async Task<List<CaiDatDtoView>> GetAsync()
        {
            var entitys = await _repo.GetsAsync();
            var data = ClassExtension.Map<CaiDatDtoView>(entitys);
            return data;
        }

        public async Task<CaiDatDtoView> CreateAsync(CaiDatDtoCreate model)
        {
            var entity = await _repo.GetAsync<CaiDatEntity>(new Dictionary<string, object>
            {
                { "ten", model.ten }, { "is_deleted", 0 }

            });
            if (entity != null)
            {
                throw new BusinessException($"Đã tồn tại đơn vị");
            }
            entity = ClassExtension.Map<CaiDatEntity>(model);
            this.ProcessInsertData(entity);
            entity.id = Guid.NewGuid();
            entity.ten_ta=this.ProcessMa(entity.ten);
            await _repo.InsertAsync(entity);
            var result = ClassExtension.Map<CaiDatDtoView>(entity);
            return result;
        }

        public async Task<CaiDatDtoView> UpdateAsync(CaiDatDtoUpdate model)
        {
            var entity = await _repo.GetAsync(model.id);
            if (entity == null)
            {
                throw new BusinessException("Notfound");
            }

            ClassExtension.Map(model, entity);
            this.ProcessUpdateData(entity);

            await _repo.UpdateAsync(entity);

            ////log
            //await this.WriteOrgLogAsync(entity.id, OrgLogType.OrgUpdate, model);

            var result = ClassExtension.Map<CaiDatDtoView>(entity);
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
