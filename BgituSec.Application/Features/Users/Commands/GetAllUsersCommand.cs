using BgituSec.Application.DTOs;
using MediatR;

namespace BgituSec.Application.Features.Users.Commands
{
    public class GetAllUsersCommand : IRequest<List<UserDTO>> {}
}
