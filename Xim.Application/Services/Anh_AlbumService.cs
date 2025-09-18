using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Xim.Application.Contracts.Anh_Album;
using Xim.Domain.Entities;
using Xim.Domain.Pagings;
using Xim.Domain.Repos;
using Xim.Library.Exceptions;
using Xim.Library.Extensions;

namespace Xim.Application.Services
{
    public class Anh_AlbumService : BaseService<IAnh_AlbumRepo>, IAnh_AlbumService
    {
        public Anh_AlbumService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<Anh_AlbumDtoView> GetAsync(Guid id)
        {
            var entity = await _repo.GetAsync(id);
            var data = ClassExtension.Map<Anh_AlbumDtoView>(entity);

            return data;
        }

        public async Task<List<Anh_AlbumDtoView>> GetAsync()
        {
            var entitys = await _repo.GetsAsync();
            var data = ClassExtension.Map<Anh_AlbumDtoView>(entitys);
            return data;
        }

        public async Task<Anh_AlbumDtoView> CreateAsync(Anh_AlbumDtoCreate model)
        {
            var entity = ClassExtension.Map<Anh_AlbumEntity>(model);
            this.ProcessInsertData(entity);
            entity.id = Guid.NewGuid();
            //entity.TenLink = this.ProcessTenLink(entity.HoVaTen);
            //entity.ViewCount = 0;
            entity.is_deleted = false;
            await _repo.InsertAsync(entity);
            var result = ClassExtension.Map<Anh_AlbumDtoView>(entity);
            return result;
        }
        public async Task<List<Anh_AlbumDtoView>> CreateAdrangeAsync(List<Anh_AlbumDtoCreate> Listmodel)
        {
            List<Anh_AlbumDtoView> DSAnh_album=new List<Anh_AlbumDtoView> ();
            foreach (var item in Listmodel)
            {
                var temp=await CreateAsync(item);
                DSAnh_album.Add(temp);
            }
            return DSAnh_album;
        }

        public async Task<Anh_AlbumDtoView> UpdateAsync(Anh_AlbumDtoUpdate model)
        {
            var entity = await _repo.GetAsync(model.id);
            if (entity == null)
            {
                throw new BusinessException("Notfound");
            }
            if (entity.LinkAnh != null && model.LinkAnh is null)
                model.LinkAnh = entity.LinkAnh;

            ClassExtension.Map(model, entity);
            this.ProcessUpdateData(entity);
            entity.is_deleted = false;
            await _repo.UpdateAsync(entity);

            var result = ClassExtension.Map<Anh_AlbumDtoView>(entity);
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
