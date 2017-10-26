namespace Oxagile.Internal.Api.Dtos
{
    public class GetCompanyDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public GetUserDto[] Users { get; set; }
    }
}