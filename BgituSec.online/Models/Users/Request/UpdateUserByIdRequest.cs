﻿using BgituSec.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace BgituSec.Api.Models.Users.Request
{
    public class UpdateUserByIdRequest
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
        public Roles? Role { get; set; }
        public bool SentNotifications { get; set; }
    }
}
