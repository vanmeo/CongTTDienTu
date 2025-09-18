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
using Xim.Application.Contracts.DMLinhvucVB;

namespace Xim.Application.Services
{
    public class DMLinhvucVBService : BaseService<IDMLinhvucVBRepo>, IDMLinhvucVBService
    {
        public DMLinhvucVBService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<DMLinhvucVBDtoView> GetAsync(Guid id)
        {
            var entity = await _repo.GetAsync(id);
            var data = ClassExtension.Map<DMLinhvucVBDtoView>(entity);
            return data;
        }

        public async Task<List<DMLinhvucVBDtoView>> GetAsync()
        {
            var entitys = await _repo.GetsAsync();
            var data = ClassExtension.Map<DMLinhvucVBDtoView>(entitys);
            return data;
        }

        public async Task<DMLinhvucVBDtoView> CreateAsync(DMLinhvucVBDtoCreate model)
        {
            var entity = await _repo.GetAsync<DMLinhvucVBEntity>(new Dictionary<string, object>
            {
                { "ten", model.ten },
            });
            if (entity != null)
            {
                throw new BusinessException($"Đã tồn tại lĩnh vực văn bản");
            }

            entity = ClassExtension.Map<DMLinhvucVBEntity>(model);
            this.ProcessInsertData(entity);
            entity.id = Guid.NewGuid();
            await _repo.InsertAsync(entity);
            var result = ClassExtension.Map<DMLinhvucVBDtoView>(entity);
            return result;
        }

        public async Task<DMLinhvucVBDtoView> UpdateAsync(DMLinhvucVBDtoUpdate model)
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

            var result = ClassExtension.Map<DMLinhvucVBDtoView>(entity);
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
