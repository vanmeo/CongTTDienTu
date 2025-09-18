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
using Xim.Application.Contracts.Menu;

namespace Xim.Application.Services
{
    public class MenuService : BaseService<IMenuRepo>, IMenuService
    {
        public MenuService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<MenuDtoView> GetAsync(Guid id)
        {
            var entity = await _repo.GetAsync(id);
            var data = ClassExtension.Map<MenuDtoView>(entity);
            return data;
        }
        public async Task<PagedResult> GetTinBaibyMenuAsync(Guid idmenu, paging paging)
        {
            var data = await _repo.GetTinBaibyMenuAsync(idmenu, paging);
            return data;
        }
        public async Task<PagedResult> GetTinBaibyMenucosubAsync(Guid idmenu, paging paging)
        {
            var data = await _repo.GetTinBaibyMenucosubAsync(idmenu,paging);
            return data;
        }
        public async Task<PagedResult> GetTaiLieubyMenuAsync(Guid idmenu, paging paging)
        {
            var data = await _repo.GetTaiLieubyMenuAsync(idmenu, paging);
            return data;
        }
        public async Task<PagedResult> GetTaiLieubyMenucosubAsync(Guid idmenu, paging paging)
        {
            var data = await _repo.GetTaiLieubyMenucosubAsync(idmenu, paging);
            return data;
        }
        public async Task<List<MenuDtoView>> GetAsync()
        {
            var entitys = await _repo.GetsAsync();
            var data = ClassExtension.Map<MenuDtoView>(entitys);
            return data;
        }
        public async Task<List<MenuWithSubMenu>> GetListMenuAsync()
        {
            var topmenu = await _repo.GetListMenuAsync();
            return topmenu;
        }

        public async Task<MenuDtoView> CreateAsync(MenuDtoCreate model)
        {
            var entity = await _repo.GetAsync<MenuEntity>(new Dictionary<string, object>
            {
                { "Ten", model.Ten },
            });
            if (entity != null)
            {
                throw new BusinessException($"Đã tồn tại menu");
            }
            entity = ClassExtension.Map<MenuEntity>(model);
            entity.tenlink = this.ProcessTenLink(entity.Ten);
           
            entity.Ma = this.ProcessMa(entity.Ten);
            this.ProcessInsertData(entity);
            entity.id = Guid.NewGuid();
            await _repo.InsertAsync(entity);
            var result = ClassExtension.Map<MenuDtoView>(entity);
            return result;
        }

        public async Task<MenuDtoView> UpdateAsync(MenuDtoUpdate model)
        {
            var entity = await _repo.GetAsync(model.id);
            if (entity == null)
            {
                throw new BusinessException("Notfound");
            }

            ClassExtension.Map(model, entity);
            this.ProcessUpdateData(entity);
            entity.tenlink = this.ProcessTenLink(entity.Ten);
            entity.Ma = this.ProcessMa(entity.Ten);
            await _repo.UpdateAsync(entity);

            ////log
            //await this.WriteOrgLogAsync(entity.id, OrgLogType.OrgUpdate, model);

            var result = ClassExtension.Map<MenuDtoView>(entity);
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
