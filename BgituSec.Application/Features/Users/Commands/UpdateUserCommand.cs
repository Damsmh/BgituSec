using BgituSec.Application.DTOs;
using BgituSec.Domain.Entities;
using MediatR;

namespace BgituSec.Application.Features.Users.Commands
{
    public class UpdateUserCommand : IRequest<UserDTO>
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public Roles? Role { get; set; }
        public bool SentNotifications { get; set; }
    }
}
