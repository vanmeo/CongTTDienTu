using API.Constants;
using FluentFTP;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Xim.AppApi.Constants;
using Xim.Application.Contracts.Album;
using Xim.Domain.Entities;
using Xim.Domain.Pagings;

namespace Xim.AppApi.Controllers
{
    [Authorize]
    public class AlbumController : BaseController
    {
        private readonly IAlbumService _service;
        public AlbumController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _service = serviceProvider.GetService<IAlbumService>();
        }
       

        /// <summary>
        /// ADMIN: Lấy danh sách album tất cả
        /// </summary>
        /// <param name="paging"> Gồm pagesize, page number</param>
        /// <returns>danh sách các album có phân trang</returns>
        [AllowAnonymous]
        [HttpPost("getall")]
        public async Task<IActionResult> GetListAsync(paging paging, bool is_video)
        {
            int offset = (paging.pageNumber - 1) * paging.pageSize;
            PagingParam param = new PagingParam();
            param.sort = "thutu";
            param.skip = offset;
            param.take = paging.pageSize;
            param.filter = $"[{{\"f\":\"is_video\",\"o\":\"=\",\"v\":{is_video.ToString().ToLower()}}}]";
            param.columns = "";
            var data = await _service.GetListAsync(param);
            dynamic sumDataDynamic = data.sumData;

            // Truy cập thuộc tính total
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
        /// ADMIN: Thêm mới một Album 
        /// </summary>
        /// <param name="model">dữ liệu cho một Album</param>
        /// <returns>Bản ghi Album vừa tạo</returns>
        [HttpPost]
        public async Task<IActionResult> InsertAsync([FromBody] AlbumDtoCreate model)
        {
            //var contextData = this.GetContext();
            //model.created_userid = contextData.UserId;
            var data = await _service.CreateAsync(model);
            return Ok(data);
        }


        /// <summary>
        /// Cổng (Sơn): Lấy danh sách các Ảnh có trong 1 album sắp xếp theo thứ tự ưu tiên 
        /// </summary>
        /// <param name="idAlbum">id Album</param>
        /// <param name="paging">pagesize, pagenumber</param>
        /// <returns>Danh sách gồm album và kèm theo là ds ảnh trong album đó</returns>
        [AllowAnonymous]
        [HttpPost("GetDSAnhByAlbum")]
        public async Task<IActionResult> GetDsAnhByAlbumAsync(Guid idAlbum, paging paging)
        {
            var data = await _service.GetListAnhByAlbum(idAlbum, paging);
            return Ok(data);
        }

        /// <summary>
        /// Admin: lấy tin chi tiết của một Album theo id
        /// </summary>
        /// <param name="id">id Album</param>
        /// <returns>Chi tiết Album</returns>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var data = await _service.GetAsync(id);
            return Ok(data);
        }
        /// <summary>
        /// Admin lưu thông tin Album muốn sửa
        /// </summary>
        /// <param name="id">idAlbum</param>
        /// <param name="model">Album cần sửa</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] AlbumDtoUpdate model)
        {
            model.id = id;
            var data = await _service.UpdateAsync(model);
            return Ok(data);
        }
        /// <summary>
        /// Admin xóa một Album
        /// </summary>
        /// <param name="id">id Album muốn xóa</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var contextData = this.GetContext();
            await _service.DeleteAsync(id);
            return Ok();
        }
    }
}
