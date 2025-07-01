using BgituSec.Application.DTOs;
using MediatR;

namespace BgituSec.Application.Features.Breakdowns.Commands
{
    public class CreateBreakdownCommand : IRequest<BreakdownDTO>
    {
        public string? Description { get; set; }
        public bool IsSolved { get; set; }
        public int Level { get; set; }
        public int ComputerId { get; set; }
        public int UserId { get; set; }
    }
}
