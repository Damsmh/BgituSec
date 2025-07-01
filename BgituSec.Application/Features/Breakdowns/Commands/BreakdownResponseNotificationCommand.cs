using BgituSec.Application.DTOs;
using MediatR;

namespace BgituSec.Application.Features.Breakdowns.Commands
{
    public class BreakdownResponseNotificationCommand(List<BreakdownDTO> breakdowns) : INotification
    {
        List<BreakdownDTO> Breakdowns { get; set; } = breakdowns;
    }
}
