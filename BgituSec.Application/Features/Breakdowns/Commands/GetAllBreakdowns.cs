using MediatR;

namespace BgituSec.Application.Features.Breakdowns.Commands
{
    public class GetAllBreakdowns : IRequest<BreakdownResponse> { }
}
