using System.ComponentModel.DataAnnotations;

namespace productExpiry_system.models
{
    public class SignUpModel
    {
        [Required]
       public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Compare("ConfirmPassword")]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
        public DateTime CreatedDate { get; set; }
        
    }
}
