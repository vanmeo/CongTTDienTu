using Xim.Domain.Entities;
using Xim.Domain.Mssql.Base;
using Xim.Domain.Mssql.Tables;
using Xim.Domain.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xim.Domain.Pagings;

namespace Xim.Domain.Mssql.Repos
{
    internal class MenuSubRepo : MssqlAppRepo<MenuSubEntity, Guid>, IMenuSubRepo
    {
        public MenuSubRepo(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
        public async Task<List<T>> GetSubMenuByMenuAsync<T>(Guid menuId)
        {
            var query =
                        $@"select * from MenuSub
                        WHERE parent_id= @menuId
                        order by thutu
                        ";
            var data = await Provider.QueryAsync<T>(query, new Dictionary<string, object>
            {
                { "menuId", menuId }
            });

            return data;
        }
        public async Task<PagedResult> GetTinBaibySubMenuAsync(Guid idmenu, paging paging)
        {

            int offset = (paging.pageNumber - 1) * paging.pageSize;

            // Query to count the total number of documents
            var countQuery = $@"
            SELECT COUNT(*)
            FROM TinTuc
            WHERE idmenu =@idmenu";

            // Execute the count query to get the total number of documents
            var totalDocuments = await Provider.QueryAsync<int>(countQuery, new Dictionary<string, object>
            {
                { "idmenu", idmenu }
            });
            int totalDocumentsCount = totalDocuments[0];
            // Query to fetch the documents with pagination
            var query = $@"
        SELECT *
        FROM TinTuc
        WHERE idmenu =@idmenu
        ORDER BY created desc
        OFFSET @offset ROWS
        FETCH NEXT @pageSize ROWS ONLY";

            // Execute the paginated query to get the documents
            var data = await Provider.QueryAsync<TinTucEntity>(query, new Dictionary<string, object>
        {
            { "idmenu", idmenu },
            { "offset", offset },
            { "pageSize", paging.pageSize }
        });

            // Calculate the total number of pages
            int totalPages = (int)Math.Ceiling((double)totalDocumentsCount / paging.pageSize);

            // Return the paginated result including data and pagination info
            return new PagedResult
            {
                Data = data.ToList(),
                PageSize = paging.pageSize,
                TotalDocuments = totalDocumentsCount,
                PageNumber = paging.pageNumber,
                TotalPages = totalPages
            };


        }
        public async Task<PagedResult> GetTaiLieubySubMenuAsync(Guid idmenu, paging paging)
        {
            // Calculate the offset based on the page number and page size
            int offset = (paging.pageNumber - 1) * paging.pageSize;

            // Query to count the total number of documents
            var countQuery = $@"
        SELECT COUNT(*)
        FROM DMTaiLieu
        WHERE idmenu =@idmenu";

            // Execute the count query to get the total number of documents
            var totalDocuments = await Provider.QueryAsync<int>(countQuery, new Dictionary<string, object>
        {
            { "idmenu", idmenu }
        });
            int totalDocumentsCount = totalDocuments[0];
            // Query to fetch the documents with pagination
            var query = $@"
        SELECT *
        FROM DMTaiLieu
        WHERE idmenu =@idmenu
        ORDER BY created desc
        OFFSET @offset ROWS
        FETCH NEXT @pageSize ROWS ONLY";

            // Execute the paginated query to get the documents
            var data = await Provider.QueryAsync<DMTailieuEntity>(query, new Dictionary<string, object>
        {
            { "idmenu", idmenu },
            { "offset", offset },
            { "pageSize", paging.pageSize }
        });

            // Calculate the total number of pages
            int totalPages = (int)Math.Ceiling((double)totalDocumentsCount / paging.pageSize);

            // Return the paginated result including data and pagination info
            return new PagedResult
            {
                Data = data.ToList(),
                PageSize = paging.pageSize,
                TotalDocuments = totalDocumentsCount,
                PageNumber = paging.pageNumber,
                TotalPages = totalPages
            };
        }
    }
}
