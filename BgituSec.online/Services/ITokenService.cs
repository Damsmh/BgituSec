using BgituSec.Application.DTOs;

namespace BgituSec.Api.Services
{
    public interface ITokenService
    {
        string CreateToken(UserDTO user);
    }
}
