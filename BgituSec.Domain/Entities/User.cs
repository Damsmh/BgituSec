﻿namespace BgituSec.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Roles Role { get; set; }
        public string Name { get; set; }

        public ICollection<Breakdown> Breakdowns { get; set; }
        public ICollection<Notification> Notifications { get; set; }
    }
}
