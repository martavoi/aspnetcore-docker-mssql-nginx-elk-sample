namespace Oxagile.Demos.Api.Dtos
{
    public class GetCompanyDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public GetCompanyUserDto[] Users { get; set; }
    }
}