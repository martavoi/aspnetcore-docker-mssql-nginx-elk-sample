using AutoMapper;
using FluentValidation;
using Oxagile.Internal.Api.Dtos;
using Oxagile.Internal.Api.Dtos.Validation;
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

            For<IValidator<CreateCompanyDto>>().Use<CreateCompanyValidator>();
            For<IValidator<EditCompanyDto>>().Use<EditCompanyValidator>();
            For<IValidator<CreateUserDto>>().Use<CreateUserValidator>();
            For<IValidator<EditUserDto>>().Use<EditUserValidator>();
        }
    }
}