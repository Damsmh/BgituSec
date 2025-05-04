using BgituSec.Application.DTOs;
using BgituSec.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BgituSec.Application.Features.Users.Commands
{
    public class LoginUserCommand : IRequest<UserDTO?>
    {
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
