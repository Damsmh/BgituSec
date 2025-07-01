using AutoMapper;
using BgituSec.Application.DTOs;
using BgituSec.Application.Features.Breakdowns.Commands;
using BgituSec.Domain.Entities;
using BgituSec.Domain.Interfaces;
using MediatR;

namespace BgituSec.Application.Features.Breakdowns.Handlers
{
    public class CreateBreakdownCommandHandler(IBreakdownRepository repository, IMapper mapper, IMediator mediator) : IRequestHandler<CreateBreakdownCommand, BreakdownDTO>
    {
        private readonly IBreakdownRepository _repository = repository;
        private readonly IMapper _mapper = mapper;
        private readonly IMediator _mediator = mediator;
        public async Task<BreakdownDTO> Handle(CreateBreakdownCommand request, CancellationToken cancellationToken)
        {
            var breakdown = _mapper.Map<Breakdown>(request);
            breakdown.CreatedAt = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time"));
            await _repository.AddAsync(breakdown);
            return _mapper.Map<BreakdownDTO>(breakdown);
        }
    }
}
