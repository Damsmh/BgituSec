namespace BgituSec.Api.Models.Users
{
    public class CreateUserResponse : UserResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
