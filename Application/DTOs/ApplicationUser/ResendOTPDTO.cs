﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.ApplicationUser
{
    public class ResendOTPDTO
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }
    }
}
