using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.ApplicationUser
{
    public class ConfirmAccountModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [RegularExpression("^[0-9]*$", ErrorMessage = "OTP can only contain numeric values.")]
        [StringLength(6, ErrorMessage = "OTP must be 6 digit long", MinimumLength = 6)]
        public string OTP { get; set; }
    }
}
