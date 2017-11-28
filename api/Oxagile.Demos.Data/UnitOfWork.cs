using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Oxagile.Demos.Data.Entities;
using Oxagile.Demos.Data.Repositories;

namespace Oxagile.Demos.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly UserCompanyContext context;

        public UnitOfWork(UserCompanyContext context)
        {
            this.context = context;
        }

        public ICompanyRepository Company => new CompanyRepository(context);
        public IUserRepository User => new UserRepository(context);
        public IUserMediaRepository Media => new UserMediaRepository(context);

        public async Task CommitAsync()
        {
            await context.SaveChangesAsync();
        }

        public async Task RejectAsync()
        {
            foreach (var entry in context.ChangeTracker.Entries()
              .Where(e => e.State != EntityState.Unchanged))
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Modified:
                    case EntityState.Deleted:
                        await entry.ReloadAsync();
                        break;
                }
            }
        }
    }
}