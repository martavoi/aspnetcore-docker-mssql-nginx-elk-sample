using AutoMapper;
using Oxagile.Internal.Api.Dtos;
using Oxagile.Internal.Api.Repositories;

namespace Oxagile.Internal.Api
{
    public class Registry: StructureMap.Registry
    {
        public Registry()
        {
            var mapper = new MapperConfiguration(_ =>
            {
                _.AddProfile<DtosMappingProfile>();
            }).CreateMapper();
            For<IMapper>().Use(mapper);

            For<ICompanyRepository>().Use<CompanyRepository>();
            For<IUserRepository>().Use<UserRepository>();
        }
    }
}