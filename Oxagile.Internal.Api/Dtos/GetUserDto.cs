using System;

namespace Oxagile.Internal.Api.Dtos
{
    public class GetUserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public GetCompanyDto Company { get; set; }
    }
}