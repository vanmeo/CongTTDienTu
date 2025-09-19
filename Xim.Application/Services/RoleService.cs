using Xim.Application.Contracts.Users;
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
using Xim.Application.Contracts.Donvis;
using Xim.Application.Contracts.Role;
using System.Net.WebSockets;
using static Dapper.SqlMapper;
using System.Collections;

namespace Xim.Application.Services
{
    public class RoleService : BaseService<IRoleRepo>, IRoleService
    {
        public RoleService(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }
        /// <summary>
        /// Lấy tất cả các role trong hệ thống
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagingData> GetListAsync(PagingParam param)
        {
            var lst = await _repo.GetPagingAsync(param);
            var sum = await _repo.GetPagingSummaryAsync(new PagingSummaryParam()
            {
                filter = param.filter
            });
            return new PagingData
            {
                data = lst,
                sumData = sum
            };
        }
        /// <summary>
        /// Thêm mới một role có list User liên quan tới Role này, chỗ này thêm get listusser by role
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task<RoleEntity> CreateAsync(RoleDtoCreate model)
        {
            var entity = await _repo.GetAsync<RoleEntity>(new Dictionary<string, object>
            {
                { "name", model.name },
            });
            if (entity != null)
            {
                throw new BusinessException($"Quyền {model.name} đã tồn tại");
            }
            entity = ClassExtension.Map<RoleEntity>(model);
            entity.id = Guid.NewGuid();
            await _repo.InsertAsync(entity);
            if (model.lst_user_id != null && model.lst_user_id != null)
            {
                var lstUserRole = new List<UserRoleEntity>();
                foreach (var item in model.lst_user_id)
                {
                    var userRole = new UserRoleEntity
                    {
                        id = Guid.NewGuid(),
                        role_id = entity.id,
                        user_id = item
                    };
                    lstUserRole.Add(userRole);
                }
                await _repo.InsertAsync<UserRoleEntity>(lstUserRole);
            }
            return entity;
        }
        public async Task DeleteAsync(Guid id)
        {
            await _repo.DeleteAsync(id);
        }
        /// <summary>
        /// Lấy role theo id
        /// </summary>
        /// <param name="id">id của role</param>
        /// <returns></returns>
        public async Task<RoleDtoView> GetAsync(Guid id)
        {
            var entity = await _repo.GetAsync(id);
            var data = ClassExtension.Map<RoleDtoView>(entity);
            if (data != null)
            {
                var userRoles = await _repo.GetsAsync<UserRoleEntity>(new Dictionary<string, object>
                {
                    {"role_id", id}
                });
                if (userRoles != null)
                {
                    var lstID = new List<Guid>();
                    foreach (UserRoleEntity item in userRoles)
                    {
                        lstID.Add(item.user_id);
                    }
                    data.lst_user_id = lstID;
                }
            }
            return data;
        }
        /// <summary>
        /// Update role, cập nhật lại cả user
        /// </summary>
        /// <returns></returns>
        public async Task<RoleDtoView> UpdateAsync(RoleDtoUpdate model)
        {
            var entity = await _repo.GetAsync(model.id);
            if (entity == null)
            {
                throw new BusinessException($"Quyền không tồn tại bạn vui lòng check lại");
            }
            // xóa hết trong bảng user_role
            await _repo.DeleteAsyncByFieldName<UserRoleEntity>(model.id, "role_id");
            // xóa xong thêm lại vào bảng user_role
            if (model.lst_user_id != null)
            {
                var lstUserRole = new List<UserRoleEntity>();
                foreach (Guid item in model.lst_user_id)
                {
                    var userRole = new UserRoleEntity
                    {
                        id = Guid.NewGuid(),
                        role_id = model.id,
                        user_id = item
                    };
                    lstUserRole.Add(userRole);
                }
                await _repo.InsertAsync<UserRoleEntity>(lstUserRole);
            }
            var data = ClassExtension.Map<RoleDtoView>(entity);
            data.lst_user_id = model.lst_user_id;
            return data;
        }
        public async Task<List<UserRoleEntity>> AddRoleForUser(UserRoleDtoCreate model)
        {
            List<UserRoleEntity> data = new List<UserRoleEntity>();
            //Xóa những user có role khác gán cho role mới
            foreach (var item in model.Luser_id)
            {
                await _repo.DeleteAsyncByFieldName<UserRoleEntity>(item, "user_id");
                var entity = ClassExtension.Map<UserRoleEntity>(new UserRoleEntity { user_id = item, role_id = model.role_id });
                this.ProcessInsertData(entity);
                entity.id = Guid.NewGuid();
                await _repo.InsertAsync(entity);
                data.Add(entity);
            }
            return data;

        }
        public async Task<List<RolePermissionEntity>> GetRolePermissionByRole(Guid role_id)
        {
            var entity = await _repo.GetsAsync<RolePermissionEntity>(new Dictionary<string, object>
            {
                {"role_id", role_id}
            });
            return entity;
        }
        public async Task<List<RolePermissionEntity>> AddRolePermission(List<RolePermisstionDtoCreate> lstModel)
        {
            //xóa hết data trong bảng rolepermisstion
            await _repo.DeleteAsync<RolePermissionEntity>(new Dictionary<string, object>{
        { "role_id", lstModel[0].role_id },
        { "module_id", lstModel[0].module_id }
        }, ConditionOperator.And);
        //await _repo.DeleteAsyncByFieldName<RolePermissionEntity>({ lstModel[0].role_id, "role_id"},{ lstModel[0].module_id, "module_id" });
        List<RolePermissionEntity> lst = new List<RolePermissionEntity>();
            foreach (RolePermisstionDtoCreate item in lstModel)
            {
                var data = new RolePermissionEntity
                {
                    id = Guid.NewGuid(),
                    quyenhan_id = item.quyenhan_id,
                    module_id = item.module_id,
                    role_id = item.role_id,
                    is_chophep = item.is_chophep
                };
        lst.Add(data);
            }
            if (lst.Count > 0)
            {
                await _repo.InsertAsync<RolePermissionEntity>(lst);
}
return lst;
        }
    }
}
