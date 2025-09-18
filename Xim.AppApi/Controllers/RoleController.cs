using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Xim.Application.Contracts.Role;
using Xim.Domain.Pagings;

namespace Xim.AppApi.Controllers
{
   // [Authorize]
    public class RoleController : BaseController
    {
        /// <summary>
        /// Quản lý các role của hệ thoogns: Biên tập, quản trị, duyệt...
        /// </summary>
        private readonly IRoleService _service;
        private readonly IModulePermissionService _servicemodulepermission;
        private readonly IUserRole _serviceuserrole;
        public RoleController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _service = serviceProvider.GetService<IRoleService>();
            _servicemodulepermission = serviceProvider.GetService<IModulePermissionService>();
            _serviceuserrole = serviceProvider.GetService<IUserRole>();
        }
        //[HttpPost("padding_filter")]
        //public async Task<IActionResult> GetListPaging(PagingParam param)
        //{
        //    var data = await _service.GetListAsync(param);
        //    return Ok(data);
        //}
        /// <summary>
        /// Admin: Hiển thị danh sách các quyền người dùng
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("getall")]
        public async Task<IActionResult> GetListAsync(paging paging)
        {
            int offset = (paging.pageNumber - 1) * paging.pageSize;
            PagingParam param = new PagingParam();
            param.sort = "created";
            param.skip = offset;
            param.take = paging.pageSize;
            param.filter = "";
            param.columns = "";
            var data = await _service.GetListAsync(param);
            dynamic sumDataDynamic = data.sumData;
            int total = sumDataDynamic.total;
            return Ok(new
            {
                Data = data.data,
                PageSize = paging.pageSize,
                TotalDocuments = total,
                PageNumber = paging.pageNumber,
                TotalPages = (int)Math.Ceiling((double)total / paging.pageSize)
            });
        }
       /// <summary>
       /// Admin: 
       /// </summary>
       /// <returns></returns>
        [HttpGet("get_list_module_permission")]
        public async Task<IActionResult> GetListModulePermission()
        {
           
            var data = await _servicemodulepermission.GetListModulePermission();
            return Ok(data);
        }
        /// <summary>
        /// Admin: Gán quyền cho user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("add_role_for_user")]
        public async Task<IActionResult> AddroleforuserAsync(UserRoleDtoCreate model)
        {
            var data = await _service.AddRoleForUser(model);
            return Ok(data);
        }
        [HttpPost]
        public async Task<IActionResult> InsertAsync([FromBody] RoleDtoCreate model)
        {
            var data = await _service.CreateAsync(model);
            return Ok(data);
        }

        ///// <summary>
        ///// xóa theo id
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {

            await _service.DeleteAsync(id);
            return Ok();
        }
        /////// <summary>
        /////// lấy chi tiết theo id
        /////// </summary>
        /////// <param name="id"></param>
        /////// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var data = await _service.GetAsync(id);
            return Ok(data);
        }
        [HttpGet("get_role_permission_by_role")]
        public async Task<IActionResult> GetRolePermissionByRole(Guid role_id)
        {
            var data = await _service.GetRolePermissionByRole(role_id);
            return Ok(data);
        }
        [HttpPost("add_role_permission")]
        public async Task<IActionResult> AddRolePermission([FromBody] List<RolePermisstionDtoCreate> model)
        {
            var data = await _service.AddRolePermission(model);
            return Ok(data);
        }
    }
}
