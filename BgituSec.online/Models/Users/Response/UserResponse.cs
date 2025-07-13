using BgituSec.Domain.Entities;

namespace BgituSec.Api.Models.Users.Response
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Roles Role { get; set; }
        public bool SentNotifications { get; set; }
    }
}
