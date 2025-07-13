using BgituSec.Application.DTOs;
using MediatR;

namespace BgituSec.Application.Features.Users.Commands
{
    public class LoginUserCommand : IRequest<UserDTO?>
    {
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
