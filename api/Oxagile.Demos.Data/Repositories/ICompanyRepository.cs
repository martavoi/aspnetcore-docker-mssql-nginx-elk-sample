using System.Collections.Generic;
using System.Threading.Tasks;
using Oxagile.Demos.Data.Entities;

namespace Oxagile.Demos.Data.Repositories
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