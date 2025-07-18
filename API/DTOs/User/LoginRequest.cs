﻿using System.ComponentModel.DataAnnotations;

namespace API.DTOs.User
{
    public class LoginRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
