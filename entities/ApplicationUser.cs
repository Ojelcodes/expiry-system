using Microsoft.AspNetCore.Identity;

namespace productExpiry_system.entities
{
    public class ApplicationUser:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
     
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
      
    }
}
