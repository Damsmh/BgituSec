using BgituSec.Application.DTOs;
using MediatR;

namespace BgituSec.Application.Features.Breakdowns.Commands
{
    public class BreakdownResponseNotification(List<BreakdownDTO> breakdowns) : INotification
    {
        public List<BreakdownDTO> Breakdowns { get; set; } = breakdowns;
    }
}
