using BgituSec.Application.DTOs;
using MediatR;


namespace BgituSec.Application.Features.Users.Commands
{
    public class DeleteUserCommand : IRequest<UserDTO>
    {
        public int Id { get; set; }
    }
}
