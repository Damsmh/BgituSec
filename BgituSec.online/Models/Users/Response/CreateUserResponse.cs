namespace BgituSec.Api.Models.Users.Response
{
    public class CreateUserResponse : UserResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
