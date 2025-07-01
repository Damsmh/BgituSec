using BgituSec.Application.DTOs;
using MediatR;

namespace BgituSec.Application.Features.Computers.Commands
{
    public class DeleteComputerCommand : IRequest<ComputerDTO>
    {
        public int Id { get; set; }
    }
}
