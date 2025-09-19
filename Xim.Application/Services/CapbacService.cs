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
using Xim.Application.Contracts.Capbacs;

namespace Xim.Application.Services
{
    public class CapbacService : BaseService<ICapbacRepo>, ICapbacService
    {
        public CapbacService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<CapbacDtoView> GetAsync(Guid id)
        {
            var entity = await _repo.GetAsync(id);
            var data = ClassExtension.Map<CapbacDtoView>(entity);
            return data;
        }

        public async Task<List<CapbacDtoView>> GetAsync()
        {
            var entitys = await _repo.GetsAsync();
            var data = ClassExtension.Map<CapbacDtoView>(entitys);
            return data;
        }

        public async Task<CapbacDtoView> CreateAsync(CapbacDtoCreate model)
        {
            var entity = await _repo.GetAsync<CapbacEntity>(new Dictionary<string, object>
            {
                { "viettat", model.viettat },
            });
            if (entity != null)
            {
                throw new BusinessException($"Đã tồn tại cấp bậc");
            }

            entity = ClassExtension.Map<CapbacEntity>(model);
            this.ProcessInsertData(entity);
            entity.id = Guid.NewGuid();
            await _repo.InsertAsync(entity);
            var result = ClassExtension.Map<CapbacDtoView>(entity);
            return result;
        }

        public async Task<CapbacDtoView> UpdateAsync(CapbacDtoUpdate model)
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

            var result = ClassExtension.Map<CapbacDtoView>(entity);
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
    }
}
