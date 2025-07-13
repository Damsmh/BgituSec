using System.ComponentModel.DataAnnotations;

namespace BgituSec.Api.Models.Users.Request
{
    public class LoginUserRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
