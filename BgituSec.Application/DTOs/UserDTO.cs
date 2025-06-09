using BgituSec.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace BgituSec.Application.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public Roles Role { get; set; }
    }
}
