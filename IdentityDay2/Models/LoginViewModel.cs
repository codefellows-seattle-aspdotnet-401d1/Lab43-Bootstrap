﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityDay2.Models
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]

        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "You do not meet the password requirements", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [DisplayName("Password")]

        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
