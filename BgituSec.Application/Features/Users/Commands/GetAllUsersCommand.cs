using BgituSec.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BgituSec.Application.Features.Users.Commands
{
    public class GetAllUsersCommand : IRequest<List<UserDTO>> {}
}
