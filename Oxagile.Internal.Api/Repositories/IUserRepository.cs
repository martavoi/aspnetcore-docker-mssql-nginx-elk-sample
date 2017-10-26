using System.Collections.Generic;
using System.Threading.Tasks;
using Oxagile.Internal.Api.Entities;

namespace Oxagile.Internal.Api.Repositories
{
    public interface IUserRepository
    {
        Task<User> Create(User c);
        Task<IEnumerable<User>> Get();
        Task<User> Get(int id);
        Task<User> Update(User c);
        Task<bool> Delete(int id);
    }
}