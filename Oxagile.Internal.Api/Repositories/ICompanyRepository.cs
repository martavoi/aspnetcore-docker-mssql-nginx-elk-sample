using System.Collections.Generic;
using System.Threading.Tasks;
using Oxagile.Internal.Api.Entities;

namespace Oxagile.Internal.Api.Repositories
{
    public interface ICompanyRepository
    {
        Task<Company> Create(Company c);
        Task<IEnumerable<Company>> Get();
        Task<Company> Get(int id);
        Task<Company> Update(Company c);
        Task<bool> Delete(int id);
    }
}