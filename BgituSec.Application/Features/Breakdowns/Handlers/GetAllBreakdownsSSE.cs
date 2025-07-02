using BgituSec.Application.Features.Breakdowns.Commands;
using MediatR;

namespace BgituSec.Application.Features.Breakdowns.Handlers
{
    public class GetAllBreakdownsSSE(IMediator mediator) : IRequestHandler<GetAllBreakdowns, BreakdownResponse>
    {
        private readonly IMediator _mediator = mediator;
        public async Task<BreakdownResponse> Handle(GetAllBreakdowns request, CancellationToken cancellationToken)
        {
            return new BreakdownResponse { breakdowns = await _mediator.Send(new GetAllBreakdownsCommand()) };
        }
    }
}
