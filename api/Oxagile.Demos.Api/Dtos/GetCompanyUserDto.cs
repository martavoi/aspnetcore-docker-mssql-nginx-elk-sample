using System;

namespace Oxagile.Demos.Api.Dtos
{
    public class GetCompanyUserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
    }
}