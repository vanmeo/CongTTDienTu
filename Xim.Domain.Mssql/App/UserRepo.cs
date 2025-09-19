using Xim.Domain.Entities;
using Xim.Domain.Mssql.Base;
using Xim.Domain.Mssql.Tables;
using Xim.Domain.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Xim.Domain.Mssql.Repos
{
    internal class UserRepo : MssqlAppRepo<UserEntity, Guid>, IUserRepo
    {
        public UserRepo(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<UserEntity> GetLoginAsync(string username)
        {
            var data = await this.GetAsync(new List<string> { "username" }, username);
            return data;
        }
        public async Task<List<T>> GetUserByDonViAsync<T>(Guid donviId)
        {
            var query =
                        $@"select * from users
                        WHERE id_donvi= @donviId
                        order by created asc
                        ";
            var data = await Provider.QueryAsync<T>(query, new Dictionary<string, object>
            {
                { "donviId", donviId }
            });

            return data;
        }
    }
}
