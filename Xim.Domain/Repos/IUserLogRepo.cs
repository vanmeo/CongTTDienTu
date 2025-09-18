using Xim.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Xim.Domain.Repos
{
    public interface IUserLogRepo : IRepo<UserLogEntity, Guid>
    {
    }
}
