using System.Threading.Tasks;
using Oxagile.Demos.Data.Repositories;

namespace Oxagile.Demos.Data
{
    public interface IUnitOfWork
    {
        ICompanyRepository Company { get; }
        IUserRepository User { get; }
        IUserMediaRepository Media { get; }

        Task CommitAsync();
        Task RejectAsync();
    }
}