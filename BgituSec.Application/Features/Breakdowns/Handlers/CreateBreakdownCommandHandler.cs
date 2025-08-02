using AutoMapper;
using BgituSec.Api.Models.Breakdowns.Response;
using BgituSec.Application.DTOs;
using BgituSec.Application.Features.Breakdowns.Commands;
using BgituSec.Application.Services.SSE;
using BgituSec.Domain.Entities;
using BgituSec.Domain.Interfaces;
using MediatR;
using System.Text.Json;

namespace BgituSec.Application.Features.Breakdowns.Handlers
{
    public class CreateBreakdownCommandHandler(IBreakdownRepository repository, IMapper mapper, IMediator mediator, ISSEService sseService) : IRequestHandler<CreateBreakdownCommand, BreakdownDTO>
    {
        private readonly IBreakdownRepository _repository = repository;
        private readonly IMapper _mapper = mapper;
        private readonly IMediator _mediator = mediator;
        private readonly ISSEService _sseService = sseService;
        public async Task<BreakdownDTO> Handle(CreateBreakdownCommand request, CancellationToken cancellationToken)
        {
            var breakdown = _mapper.Map<Breakdown>(request);
            breakdown.CreatedAt = DateTime.UtcNow;
            await _repository.AddAsync(breakdown);
            var message = JsonSerializer.Serialize(_mapper.Map<GetBreakdownResponseSSE>(await _mediator.Send(new GetAllBreakdowns(), CancellationToken.None)));
            await _sseService.NotifyClientsAsync(message);

            return _mapper.Map<BreakdownDTO>(breakdown);
        }
    }
}
