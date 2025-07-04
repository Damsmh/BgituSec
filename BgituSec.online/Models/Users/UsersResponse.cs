﻿using BgituSec.Domain.Entities;

namespace BgituSec.Api.Models.Users
{
    public class UsersResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Roles Role { get; set; }
    }
}
