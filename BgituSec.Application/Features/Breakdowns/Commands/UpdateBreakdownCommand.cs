using BgituSec.Application.DTOs;
using MediatR;

namespace BgituSec.Application.Features.Breakdowns.Commands
{
    public class UpdateBreakdownCommand : IRequest<BreakdownDTO>
    {
        public int Id { get; set; }
        public bool IsSolved { get; set; }
    }
}
