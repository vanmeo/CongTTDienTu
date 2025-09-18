using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Xim.Application.Contracts.Users;
using Xim.Domain.Pagings;

namespace Xim.AppApi.Controllers
{
  //  [Authorize]
    public class UserController : BaseController
    {
        private readonly IUserService _service;
        public UserController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _service = serviceProvider.GetService<IUserService>();
        }

        [HttpPost("filter")]
        public async Task<IActionResult> GetListAsync(PagingParam param)
        {
            var data = await _service.GetListAsync(param);
            return Ok(data);
        }
        [HttpPost("getall")]

        public async Task<IActionResult> GetListAsync(paging paging)
        {
            int offset = (paging.pageNumber - 1) * paging.pageSize;
            PagingParam param = new PagingParam();
            param.sort = "created";
            param.skip = offset;
            param.take = paging.pageSize;
            param.filter = "[{ 'f':'is_deleted','o':'=','v':'0'}]";
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


        [HttpPost]
        public async Task<IActionResult> InsertAsync([FromBody] UserDtoCreate model)
        {
            //var contextData = this.GetContext();
            //model.created_userid = contextData.UserId;

            var data = await _service.CreateAsync(model);
            return Ok(data);
        }
        [HttpGet("ByDonvi")]
        public async Task<IActionResult> GetbydonviAsync(Guid iddonvi)
        {
            var data = await _service.GetByDonviAsync(iddonvi);
            return Ok(data);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var data = await _service.GetAsync(id);
            return Ok(data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UserDtoUpdate model)
        {
            model.id = id;
            var data = await _service.UpdateAsync(model);
            return Ok(data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var contextData = this.GetContext();
            await _service.DeleteAsync(id);
            return Ok();
        }
    }
}
