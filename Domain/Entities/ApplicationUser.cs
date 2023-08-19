using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime? DateUpdated { get; set; } = DateTime.Now;
        public string RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public bool IsDeactivated { get; set; }
        public DateTime? LastLoginDate { get; set; }
        [ForeignKey("Store")]
        public int StoreId { get; set; }
        public Store Store { get; set; }
    }
}
