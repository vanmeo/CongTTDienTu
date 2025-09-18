using Xim.Application.Contracts.Chucvus;
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
    public class ChucvuService : BaseService<IChucvuRepo>, IChucvuService
    {
        public ChucvuService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<ChucvuDtoView> GetAsync(Guid id)
        {
            var entity = await _repo.GetAsync(id);
            var data = ClassExtension.Map<ChucvuDtoView>(entity);
            return data;
        }

        public async Task<List<ChucvuDtoView>> GetAsync()
        {
            var entitys = await _repo.GetsAsync();
            var data = ClassExtension.Map<ChucvuDtoView>(entitys);
            return data;
        }

        public async Task<ChucvuDtoView> CreateAsync(ChucvuDtoCreate model)
        {
            var entity = await _repo.GetAsync<ChucvuEntity>(new Dictionary<string, object>
            {
                { "viettat", model.viettat },
            });
            if (entity != null)
            {
                throw new BusinessException($"Đã tồn tại Chức vụ này");
            }

            entity = ClassExtension.Map<ChucvuEntity>(model);
            this.ProcessInsertData(entity);
            entity.id = Guid.NewGuid();
            await _repo.InsertAsync(entity);
            var result = ClassExtension.Map<ChucvuDtoView>(entity);
            return result;
        }

        public async Task<ChucvuDtoView> UpdateAsync(ChucvuDtoUpdate model)
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

            var result = ClassExtension.Map<ChucvuDtoView>(entity);
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
