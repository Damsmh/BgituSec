using System.ComponentModel.DataAnnotations;

namespace BgituSec.Api.Models.Users
{
    public class GetUserRequest
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
    }
}
