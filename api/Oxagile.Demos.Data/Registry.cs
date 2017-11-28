using Oxagile.Demos.Data.Repositories;

namespace Oxagile.Demos.Data
{
    public class Registry: StructureMap.Registry
    {
        public Registry()
        {
            For<ICompanyRepository>().Use<CompanyRepository>();
            For<IUserRepository>().Use<UserRepository>();
            For<IUserMediaRepository>().Use<UserMediaRepository>();
            For<IUnitOfWork>().Use<UnitOfWork>();
        }
    }
}