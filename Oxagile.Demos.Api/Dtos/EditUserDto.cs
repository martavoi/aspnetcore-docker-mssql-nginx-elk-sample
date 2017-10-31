using System;

namespace Oxagile.Demos.Api.Dtos
{
    public class EditUserDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public int CompanyId { get; set; }
    }
}