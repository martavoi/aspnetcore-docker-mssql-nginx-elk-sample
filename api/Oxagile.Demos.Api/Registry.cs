using AutoMapper;
using FluentValidation;
using Oxagile.Demos.Api.Dtos;
using Oxagile.Demos.Api.Dtos.Validation;
using Oxagile.Demos.Data.Repositories;
using Oxagile.Demos.Api.Services;

namespace Oxagile.Demos.Api
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
            For<IUserMediaRepository>().Use<UserMediaRepository>();

            For<IBlobStorage>().Use<BlobStorage>();
            For<IImageProcessor>().Use<ImageProcessor>();

            For<IValidator<CreateCompanyDto>>().Use<CreateCompanyValidator>();
            For<IValidator<EditCompanyDto>>().Use<EditCompanyValidator>();
            For<IValidator<CreateUserDto>>().Use<CreateUserValidator>();
            For<IValidator<EditUserDto>>().Use<EditUserValidator>();
        }
    }
}