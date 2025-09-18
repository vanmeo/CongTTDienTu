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
    internal class AlbumRepo : MssqlAppRepo<AlbumEntity, Guid>, IAlbumRepo
    {
        public AlbumRepo(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
       


        public async Task<List<AlbumWithAnh>> GetListAlbumAsync()
        {
            var query =
                        $@"select * from album
                        order by thutu
                        ";
            var dataAlbum = await Provider.QueryAsync<AlbumEntity>(query, new Dictionary<string, object>
            {

            });
            var data = new List<AlbumWithAnh>();
            foreach (var item in dataAlbum)
            {
                query =
                        $@"select * from Anh_Album
                        WHERE idalbum=@idAlbum
                        order by thutu
                        ";
                var dataAnh = await Provider.QueryAsync<Anh_AlbumEntity>(query, new Dictionary<string, object>
                {
                     { "idAlbum", item.id }
                });
                var thutruongwithDM = new AlbumWithAnh
                {
                    album = item,
                    DanhsachAnh_Album = dataAnh.ToList()
                };

                data.Add(thutruongwithDM);

            }
            return data;
        }
       
       
        public async Task<PagedResult> GetListAnhByAlbum(Guid idAlbum, paging paging)
        {
            // Calculate the offset based on the page number and page size
            int offset = (paging.pageNumber - 1) * paging.pageSize;

            // Query to count the total number of documents
            var countQuery = $@"
        SELECT COUNT(*)
        FROM Anh_Album
        WHERE idalbum = @idAlbum and is_deleted=0";

            // Execute the count query to get the total number of documents
            var totalDocuments = await Provider.QueryAsync<int>(countQuery, new Dictionary<string, object>
        {
            { "idAlbum", idAlbum }
        });
            int totalDocumentsCount = totalDocuments[0];
            // Query to fetch the documents with pagination
            var query = $@"
        SELECT *
        FROM Anh_Album
        WHERE idAlbum = @idAlbum and is_deleted=0
        ORDER BY thutu
        OFFSET @offset ROWS
        FETCH NEXT @pageSize ROWS ONLY";

            // Execute the paginated query to get the documents
            var data = await Provider.QueryAsync<ThuTruongBQPEntity>(query, new Dictionary<string, object>
    {
        { "idAlbum", idAlbum },
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
