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
using Xim.Application.Contracts.DMCoQuanBHVB;

namespace Xim.Application.Services
{
    public class DMCoQuanBHVBService : BaseService<IDMCoQuanBHVBRepo>, IDMCoQuanBHVBService
    {
        public DMCoQuanBHVBService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<DMCoQuanBHVBDtoView> GetAsync(Guid id)
        {
            var entity = await _repo.GetAsync(id);
            var data = ClassExtension.Map<DMCoQuanBHVBDtoView>(entity);
            return data;
        }

        public async Task<List<DMCoQuanBHVBDtoView>> GetAsync()
        {
            var entitys = await _repo.GetsAsync();
            var data = ClassExtension.Map<DMCoQuanBHVBDtoView>(entitys);
            return data;
        }

        public async Task<DMCoQuanBHVBDtoView> CreateAsync(DMCoQuanBHVBDtoCreate model)
        {
            var entity = await _repo.GetAsync<DMCoQuanBHVBEntity>(new Dictionary<string, object>
            {
                { "ten", model.ten },
            });
            if (entity != null)
            {
                throw new BusinessException($"Đã tồn tại cơ quan ban hành");
            }

            entity = ClassExtension.Map<DMCoQuanBHVBEntity>(model);
            this.ProcessInsertData(entity);
            entity.id = Guid.NewGuid();
            await _repo.InsertAsync(entity);
            var result = ClassExtension.Map<DMCoQuanBHVBDtoView>(entity);
            return result;
        }

        public async Task<DMCoQuanBHVBDtoView> UpdateAsync(DMCoQuanBHVBDtoUpdate model)
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

            var result = ClassExtension.Map<DMCoQuanBHVBDtoView>(entity);
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
