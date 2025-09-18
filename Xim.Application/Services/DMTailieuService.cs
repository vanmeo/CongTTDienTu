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
using Xim.Application.Contracts.DMTailieu;

namespace Xim.Application.Services
{
    public class DMTailieuService : BaseService<IDMTailieuRepo>, IDMTailieuService
    {
        public DMTailieuService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<DMTailieuDtoView> GetAsync(Guid id)
        {
            var entity = await _repo.GetAsync(id);
            var data = ClassExtension.Map<DMTailieuDtoView>(entity);
            return data;
        }

        public async Task<List<DMTailieuDtoView>> GetAsync()
        {
            var entitys = await _repo.GetsAsync();
            var data = ClassExtension.Map<DMTailieuDtoView>(entitys);
            return data;
        }

        public async Task<DMTailieuDtoView> CreateAsync(DMTailieuDtoCreate model)
        {
            var entity = await _repo.GetAsync<DMTailieuEntity>(new Dictionary<string, object>
            {
                { "title", model.title },
            });
            if (entity != null)
            {
                throw new BusinessException($"Đã tồn tại Tài liệu");
            }
            entity = ClassExtension.Map<DMTailieuEntity>(model);
            this.ProcessInsertData(entity);
            entity.id = Guid.NewGuid();
            entity.is_deleted = false;
            await _repo.InsertAsync(entity);
            var result = ClassExtension.Map<DMTailieuDtoView>(entity);
            return result;
        }

        public async Task<DMTailieuDtoView> UpdateAsync(DMTailieuDtoUpdate model)
        {
            var entity = await _repo.GetAsync(model.id);
            if (entity == null)
            {
                throw new BusinessException("Notfound");
            }
            if (entity.file_url != null && model.file_url is null)
            {
                model.file_url = entity.file_url;
                model.filename = entity.filename;
            }
            ClassExtension.Map(model, entity);
            this.ProcessUpdateData(entity);

            await _repo.UpdateAsync(entity);

            ////log
            //await this.WriteOrgLogAsync(entity.id, OrgLogType.OrgUpdate, model);

            var result = ClassExtension.Map<DMTailieuDtoView>(entity);
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
