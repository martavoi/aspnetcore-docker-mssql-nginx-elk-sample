using System.Collections.Generic;
using System.Threading.Tasks;
using Oxagile.Demos.Data.Entities;

namespace Oxagile.Demos.Data.Repositories
{
    public interface IUserRepository
    {
        Task<User> Create(User c);
        Task<IEnumerable<User>> Get();
        Task<User> Get(int id);
        User Update(User c);
        void Delete(User id);
    }
}