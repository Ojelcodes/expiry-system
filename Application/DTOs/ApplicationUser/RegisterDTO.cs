using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.ApplicationUser
{
    public class RegisterDTO
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required, EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string StoreName { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public bool AgreeToAllPolicies { get; set; }
    }
}