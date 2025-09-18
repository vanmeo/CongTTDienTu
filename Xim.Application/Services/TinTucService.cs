using Xim.Domain.Entities;
using Xim.Domain.Repos;
using Xim.Library.Exceptions;
using Xim.Library.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xim.Domain.Pagings;
using Xim.Application.Contracts.TinTuc;

namespace Xim.Application.Services
{
    public class TinTucService : BaseService<ITinTucRepo>, ITinTucService
    {
        public TinTucService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<TinTucDtoView> GetAsync(Guid id)
        {
            var entity = await _repo.GetAsync(id);
            var data = ClassExtension.Map<TinTucDtoView>(entity);
            
            return data;
        }

        public async Task<List<TinTucDtoView>> GetAsync()
        {
            var entitys = await _repo.GetsAsync();
            var data = ClassExtension.Map<TinTucDtoView>(entitys);
            return data;
        }

        public async Task<TinTucDtoView> CreateAsync(TinTucDtoCreate model)
        {
            //var entity = await _repo.GetAsync<TinTucEntity>(new Dictionary<string, object>
            //{
            //    { "TieuDe", model.TieuDe },
            //});
            //if (entity != null)
            //{
            //    throw new BusinessException($"Đã tồn tại bài viết");
            //}

           var entity = ClassExtension.Map<TinTucEntity>(model);
            this.ProcessInsertData(entity);
            if (entity.TinhTrang == 1)
                entity.NgayPH = DateTime.Now;
            entity.id = Guid.NewGuid();
            entity.Link = this.ProcessTenLink(entity.TieuDe);
            entity.ViewCount = 0;
            entity.is_deleted = false;
            await _repo.InsertAsync(entity);
            var result = ClassExtension.Map<TinTucDtoView>(entity);
            return result;
        }

        public async Task<TinTucDtoView> UpdateAsync(TinTucDtoUpdate model)
        {
            var entity = await _repo.GetAsync(model.id);
            if (entity == null)
            {
                throw new BusinessException("Notfound");
            }
            if (entity.AnhChinh != null && model.AnhChinh is null)
            {
                model.AnhChinh = entity.AnhChinh;
            }

            ClassExtension.Map(model, entity);
            this.ProcessUpdateData(entity);
            if (entity.TinhTrang == 1&&entity.NgayPH!=null)
                entity.NgayPH = DateTime.Now;
            entity.Link = this.ProcessTenLink(entity.TieuDe);
            await _repo.UpdateAsync(entity);

            ////log
            //await this.WriteOrgLogAsync(entity.id, OrgLogType.OrgUpdate, model);

            var result = ClassExtension.Map<TinTucDtoView>(entity);
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
