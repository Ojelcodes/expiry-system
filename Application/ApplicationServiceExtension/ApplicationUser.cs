using Microsoft.AspNetCore.Identity;

namespace productExpiry_system.entities
{
    public class ApplicationUser : IdentityUser<long>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public bool IsDeactivated { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime? DateUpdated { get; set; }

    }
}
