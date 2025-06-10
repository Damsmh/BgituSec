using Microsoft.EntityFrameworkCore;

namespace BgituSec.Domain.Entities
{
    [Index(nameof(Name), Name = "IX_Name", IsUnique = true)]
    public class User
    {
        public string Email { get; set; }
        public int Id { get; set; }
        public string Password { get; set; }
        public Roles Role { get; set; }
        public string Name { get; set; }

        public ICollection<Breakdown> Breakdowns { get; set; }
        public ICollection<Notification> Notifications { get; set; }
    }
}
