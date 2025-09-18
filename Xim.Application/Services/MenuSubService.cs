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
using Xim.Application.Contracts.MenuSub;


namespace Xim.Application.Services
{
    public class MenuSubService : BaseService<IMenuSubRepo>, IMenuSubService
    {
        readonly IMenuRepo _menuRepo;
        public MenuSubService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _menuRepo = serviceProvider.GetService<IMenuRepo>();
        }

        public async Task<MenuSubDtoView> GetAsync(Guid id)
        {
            var entity = await _repo.GetAsync(id);
            var data = ClassExtension.Map<MenuSubDtoView>(entity);
            return data;
        }

        public async Task<List<MenuSubDtoView>> GetAsync()
        {
            var entitys = await _repo.GetsAsync();
            var data = ClassExtension.Map<MenuSubDtoView>(entitys);
            return data;
        }

        public async Task<MenuSubDtoView> CreateAsync(MenuSubDtoCreate model)
        {
            var entity = await _repo.GetAsync<MenuSubEntity>(new Dictionary<string, object>
            {
                { "Ten", model.Ten },
            });
            if (entity != null)
            {
                throw new BusinessException($"Đã tồn tại MenuSub");
            }

            entity = ClassExtension.Map<MenuSubEntity>(model);
            this.ProcessInsertData(entity);
            entity.id = Guid.NewGuid();
            entity.tenlink = this.ProcessTenLink(entity.Ten);
            entity.Ma = this.ProcessMa(entity.Ten);
            if(entity.parent_id.HasValue)
            {
                MenuEntity menuEntity = await _menuRepo.GetAsync(entity.parent_id.Value);
                entity.iType = menuEntity.iType;
            }    
            await _repo.InsertAsync(entity);
            var result = ClassExtension.Map<MenuSubDtoView>(entity);
            return result;
        }
        public async Task<PagedResult> GetTinBaibySubMenuAsync(Guid idmenu, paging paging)
        {
            var data = await _repo.GetTinBaibySubMenuAsync(idmenu, paging);
            return data;
        }
        public async Task<PagedResult> GetTaiLieubySubMenuAsync(Guid idmenu, paging paging)
        {
            var data = await _repo.GetTaiLieubySubMenuAsync(idmenu, paging);
            return data;
        }

        public async Task<MenuSubDtoView> UpdateAsync(MenuSubDtoUpdate model)
        {
            var entity = await _repo.GetAsync(model.id);
            if (entity == null)
            {
                throw new BusinessException("Notfound");
            }

            ClassExtension.Map(model, entity);
            if (entity.parent_id.HasValue)
            {
                MenuEntity menuEntity = await _menuRepo.GetAsync(entity.parent_id.Value);
                entity.iType = menuEntity.iType;
            }
            this.ProcessUpdateData(entity);
            entity.tenlink = this.ProcessTenLink(entity.Ten);
            entity.Ma = this.ProcessMa(entity.Ten);
            await _repo.UpdateAsync(entity);

            ////log
            //await this.WriteOrgLogAsync(entity.id, OrgLogType.OrgUpdate, model);

            var result = ClassExtension.Map<MenuSubDtoView>(entity);
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
        public async Task<List<MenuSubDtoView>> GetbyMenuIdAsync(Guid menuid)
        {
            var entitys = await _repo.GetSubMenuByMenuAsync<MenuSubEntity>(menuid);
            var data = ClassExtension.Map<MenuSubDtoView>(entitys);
            return data;
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
