using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xim.Application.Contracts.Album;
using Xim.Application.Contracts.Donvis;
using Xim.Application.Contracts.Menu;
using Xim.Domain.Entities;
using Xim.Domain.Pagings;
using Xim.Domain.Repos;
using Xim.Library.Constants;
using Xim.Library.Exceptions;
using Xim.Library.Extensions;

namespace Xim.Application.Services
{
    public class AlbumService : BaseService<IAlbumRepo>, IAlbumService

    {
        public AlbumService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<AlbumDtoView> GetAsync(Guid id)
        {
            var entity = await _repo.GetAsync(id);
            var data = ClassExtension.Map<AlbumDtoView>(entity);
            return data;
        }
        public async Task<List<AlbumDtoView>> GetAsync()
        {
            var entitys = await _repo.GetsAsync();
            var data = ClassExtension.Map<AlbumDtoView>(entitys);
            return data;
        }
        public async Task<AlbumDtoView> CreateAsync(AlbumDtoCreate model)
        {
            var entity = await _repo.GetAsync<AlbumEntity>(new Dictionary<string, object>
            {
                { "Tenalbum", model.Tenalbum },
            });
            if (entity != null)
            {
                throw new BusinessException($"Đã tồn tại Album");
            }
            entity = ClassExtension.Map<AlbumEntity>(model);
            this.ProcessInsertData(entity);
            entity.id = Guid.NewGuid();
            await _repo.InsertAsync(entity);
            var result = ClassExtension.Map<AlbumDtoView>(entity);
            return result;
        }

        public async Task<AlbumDtoView> UpdateAsync(AlbumDtoUpdate model)
        {
            var entity = await _repo.GetAsync(model.id);
            if (entity == null)
            {
                throw new BusinessException("Notfound");
            }

            ClassExtension.Map(model, entity);
            this.ProcessUpdateData(entity);
            entity.is_deleted = false;
   
            await _repo.UpdateAsync(entity);

            ////log
            //await this.WriteOrgLogAsync(entity.id, OrgLogType.OrgUpdate, model);

            var result = ClassExtension.Map<AlbumDtoView>(entity);
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



        public async Task<List<AlbumWithAnh>> GetListAlbumAsync()
        {
            var data = await _repo.GetListAlbumAsync();
            return data;
        }
        public async Task<PagedResult> GetListAnhByAlbum(Guid idAlbum, paging paging)
        {
            var data = await _repo.GetListAnhByAlbum(idAlbum, paging);
            return data;
        }

    }
}
