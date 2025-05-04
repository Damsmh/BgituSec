using BgituSec.Application.DTOs;
using MediatR;

namespace BgituSec.Application.Features.Users.Commands
{
    public class CreateUserCommand : IRequest<UserDTO>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Name { get; set; }
    }
}
