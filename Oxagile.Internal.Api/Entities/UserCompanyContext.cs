using Microsoft.EntityFrameworkCore;

namespace Oxagile.Internal.Api.Entities
{
    public class UserCompanyContext: DbContext
    {
        public UserCompanyContext(DbContextOptions options)
            :base(options)
        {
            
        }
        
        public DbSet<Company> Companies { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserMedia> UserMedia { get; set; }
    }
}