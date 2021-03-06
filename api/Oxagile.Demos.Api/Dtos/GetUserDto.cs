using System;

namespace Oxagile.Demos.Api.Dtos
{
    public class GetUserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public string PicUrl { get; internal set; }
        public GetUserCompanyDto Company { get; set; }
    }
}