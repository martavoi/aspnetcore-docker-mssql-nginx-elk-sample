using System;

namespace Oxagile.Internal.Api.Dtos
{
    public class EditUserDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public EditCompanyDto Company { get; set; }
    }
}