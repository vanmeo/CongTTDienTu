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
    internal class DMThuTruongRepo : MssqlAppRepo<DMThuTruongEntity, Guid>, IDMThuTruongRepo
    {
        public DMThuTruongRepo(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<List<DMWithTenThuTruong>> GetListThuTruongAsync()
        {
            var query =
                        $@"select * from DMThuTruong
                        order by thutu
                        ";
            var dataMenu = await Provider.QueryAsync<DMThuTruongEntity>(query, new Dictionary<string, object>
            {

            });
            var data = new List<DMWithTenThuTruong>();
            foreach (var item in dataMenu)
            {
                query =
                        $@"select * from ThuTruongBQP
                        WHERE idDMThuTruong=@idDMThuTruong
                        order by thutu
                        ";
                var dataThuTruong = await Provider.QueryAsync<ThuTruongBQPEntity>(query, new Dictionary<string, object>
                {
                     { "idDMThuTruong", item.id }
                });
                var thutruongwithDM = new DMWithTenThuTruong
                {
                    DMThuTruong = item,
                    DanhsachThutruong = dataThuTruong.ToList()
                };

                data.Add(thutruongwithDM);

            }
            return data;
        }
       
       
        public async Task<PagedResult> GetThuTruongByChuVuAsync(Guid idDMThuTruong, paging paging)
        {
            // Calculate the offset based on the page number and page size
            int offset = (paging.pageNumber - 1) * paging.pageSize;

            // Query to count the total number of documents
            var countQuery = $@"
        SELECT COUNT(*)
        FROM ThuTruongBQP
        WHERE idDMThuTruong = @idDMThuTruong and is_deleted=0";

            // Execute the count query to get the total number of documents
            var totalDocuments = await Provider.QueryAsync<int>(countQuery, new Dictionary<string, object>
        {
            { "idDMThuTruong", idDMThuTruong }
        });
            int totalDocumentsCount = totalDocuments[0];
            // Query to fetch the documents with pagination
            var query = $@"
        SELECT *
        FROM ThuTruongBQP
        WHERE idDMThuTruong = @idDMThuTruong and is_deleted=0
        ORDER BY thutu
        OFFSET @offset ROWS
        FETCH NEXT @pageSize ROWS ONLY";

            // Execute the paginated query to get the documents
            var data = await Provider.QueryAsync<ThuTruongBQPEntity>(query, new Dictionary<string, object>
    {
        { "idDMThuTruong", idDMThuTruong },
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
