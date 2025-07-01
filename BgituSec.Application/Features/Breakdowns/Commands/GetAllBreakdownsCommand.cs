using BgituSec.Application.DTOs;
using MediatR;

namespace BgituSec.Application.Features.Breakdowns.Commands
{
    public class GetAllBreakdownsCommand : IRequest<List<BreakdownDTO>> { }
}
