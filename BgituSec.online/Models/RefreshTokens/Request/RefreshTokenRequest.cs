using System.ComponentModel.DataAnnotations;

namespace BgituSec.Api.Models.RefreshTokens.Request
{
    public class RefreshTokenRequest
    {
        [Required]
        public string Token { get; set; }
        [Required]
        public string RefreshToken { get; set; }
    }
}
