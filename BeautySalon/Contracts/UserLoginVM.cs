﻿using BeautySalon.Constants;
using System.ComponentModel.DataAnnotations;

namespace BeautySalon.Contracts
{
    public class UserLoginVM
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public bool RememberLogin { get; set; }

        public string? ReturnUrl { get; set; }
    }
}
