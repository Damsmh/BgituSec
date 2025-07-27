using AutoMapper;
using BgituSec.Api.Models.Breakdowns.Response;
using BgituSec.Application.Features.Breakdowns.Commands;
using MediatR;

namespace BgituSec.Application.Features.Breakdowns.Handlers
{
    public class GetAllBreakdownsSSE(IMediator mediator, IMapper mapper) : IRequestHandler<GetAllBreakdowns, BreakdownResponse>
    {
        private readonly IMediator _mediator = mediator;
        private readonly IMapper _mapper = mapper;
        public async Task<BreakdownResponse> Handle(GetAllBreakdowns request, CancellationToken cancellationToken)
        {
            return new BreakdownResponse { breakdowns = await _mediator.Send(new GetAllBreakdownsCommand()) };
        }
    }
}
