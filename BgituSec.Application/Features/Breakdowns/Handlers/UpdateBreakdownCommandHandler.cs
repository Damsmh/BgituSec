using AutoMapper;
using BgituSec.Api.Models.Breakdowns.Response;
using BgituSec.Application.DTOs;
using BgituSec.Application.Features.Breakdowns.Commands;
using BgituSec.Application.Services.SSE;
using BgituSec.Domain.Interfaces;
using MediatR;
using System.Text.Json;

namespace BgituSec.Application.Features.Breakdowns.Handlers
{
    public class UpdateBreakdownCommandHandler(IBreakdownRepository repository, IMapper mapper, IMediator mediator, ISSEService sseService) : IRequestHandler<UpdateBreakdownCommand, BreakdownDTO>
    {
        private readonly IBreakdownRepository _repository = repository;
        private readonly IMapper _mapper = mapper;
        private readonly IMediator _mediator = mediator;
        private readonly ISSEService _sseService = sseService;
        public async Task<BreakdownDTO> Handle(UpdateBreakdownCommand request, CancellationToken cancellationToken)
        {
            var breakdown = await _repository.GetByIdAsync(request.Id);
            breakdown.IsSolved = request.IsSolved;
            await _repository.UpdateAsync(breakdown);
            var message = JsonSerializer.Serialize(_mapper.Map<GetBreakdownResponseSSE>(await _mediator.Send(new GetAllBreakdowns(), CancellationToken.None)));
            await _sseService.NotifyClientsAsync(message);
            return _mapper.Map<BreakdownDTO>(breakdown);
        }
    }
}
