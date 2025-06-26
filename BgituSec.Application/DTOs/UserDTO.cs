using BgituSec.Domain.Entities;

namespace BgituSec.Application.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Roles Role { get; set; }
    }
}
