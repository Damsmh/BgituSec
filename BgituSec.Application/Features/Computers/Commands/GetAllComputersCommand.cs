using BgituSec.Application.DTOs;
using MediatR;

namespace BgituSec.Application.Features.Computers.Commands
{
    public class GetAllComputersCommand : IRequest<List<ComputerDTO>> { }
}
