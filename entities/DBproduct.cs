using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace productExpiry_system.entities
{
    public class DBproduct : IdentityDbContext<ApplicationUser>
    {
        public DBproduct(DbContextOptions<DBproduct> options) : base(options)
        {

        }
        
       public DbSet<Product> Products { get; set; }
    }
}
