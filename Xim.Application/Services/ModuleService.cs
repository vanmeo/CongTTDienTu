using Xim.Application.Contracts.Module;
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

namespace Xim.Application.Services
{
    public class ModuleService : BaseService<IModuleRepo>, IModuleService
    {
        public ModuleService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<ModuleDtoView> GetAsync(Guid id)
        {
            var entity = await _repo.GetAsync(id);
            var data = ClassExtension.Map<ModuleDtoView>(entity);
            return data;
        }

        public async Task<List<ModuleDtoView>> GetAsync()
        {
            var entitys = await _repo.GetsAsync();
            var data = ClassExtension.Map<ModuleDtoView>(entitys);
            return data;
        }

        public async Task<ModuleDtoView> CreateAsync(ModuleDtoCreate model)
        {
            var entity = await _repo.GetAsync<ModuleEntity>(new Dictionary<string, object>
            {
                { "ten", model.ten },
            });
            if (entity != null)
            {
                throw new BusinessException($"Đã tồn tại đơn vị");
            }
            entity = ClassExtension.Map<ModuleEntity>(model);
            this.ProcessInsertData(entity);
            entity.id = Guid.NewGuid();
            await _repo.InsertAsync(entity);
            var result = ClassExtension.Map<ModuleDtoView>(entity);
            return result;
        }

        public async Task<ModuleDtoView> UpdateAsync(ModuleDtoUpdate model)
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

            var result = ClassExtension.Map<ModuleDtoView>(entity);
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
            //await this.WriteOrgLogAsync(model.created_userid.Value, OrgLogType.OrgDelete, new
            //{
            //    model.id
            //});
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
