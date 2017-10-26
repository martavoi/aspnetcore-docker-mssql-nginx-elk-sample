using AutoMapper;
using Oxagile.Internal.Api.Entities;

namespace Oxagile.Internal.Api.Dtos
{
    public class DtosMappingProfile : Profile
    {
        public DtosMappingProfile()
        {
            CreateMap<Company, GetCompanyDto>();
            CreateMap<CreateCompanyDto, Company>();
            CreateMap<Company, EditCompanyDto>().ReverseMap();
            CreateMap<User, GetUserDto>();
            CreateMap<User, CreateCompanyDto>();
            CreateMap<User, EditUserDto>();
            CreateMap<User, EditCompanyUserDto>().ReverseMap();
        }
    }
}