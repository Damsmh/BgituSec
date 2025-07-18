﻿using Microsoft.EntityFrameworkCore;

namespace BgituSec.Domain.Entities
{
    [Index(nameof(UserId), Name = "IX_UserId", IsUnique = false)]
    public class RefreshToken
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime? RevokedAt { get; set; }

        public User User { get; set; }
    }
}
