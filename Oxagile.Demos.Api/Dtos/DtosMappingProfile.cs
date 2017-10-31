using AutoMapper;
using Oxagile.Demos.Data.Entities;

namespace Oxagile.Demos.Api.Dtos
{
    public class DtosMappingProfile : Profile
    {
        public DtosMappingProfile()
        {
            CreateMap<Company, GetCompanyDto>();
            CreateMap<CreateCompanyDto, Company>();
            CreateMap<Company, EditCompanyDto>().ReverseMap();
            CreateMap<Company, GetUserCompanyDto>();

            CreateMap<User, GetUserDto>();
            CreateMap<EditUserDto, User>();
            CreateMap<CreateUserDto, User>();
            CreateMap<User, EditCompanyUserDto>().ReverseMap();
            CreateMap<User, GetCompanyUserDto>();
        }
    }
}