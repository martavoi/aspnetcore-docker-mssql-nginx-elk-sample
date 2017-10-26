using System;

namespace Oxagile.Internal.Api.Dtos
{
    public class CreateUserDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public CreateCompanyDto Company { get; set; }
    }
}