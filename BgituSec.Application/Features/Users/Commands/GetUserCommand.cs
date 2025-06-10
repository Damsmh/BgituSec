using BgituSec.Application.DTOs;
using MediatR;

namespace BgituSec.Application.Features.Users.Commands
{
    public class GetUserCommand : IRequest<UserDTO?>
    {
        public int Id { get; set; }
    }
}
