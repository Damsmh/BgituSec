using AutoMapper;
using BgituSec.Application.DTOs;
using BgituSec.Application.Features.Breakdowns.Commands;
using BgituSec.Domain.Entities;
using BgituSec.Domain.Interfaces;
using MediatR;

namespace BgituSec.Application.Features.Breakdowns.Handlers
{
    public class UpdateBreakdownCommandHandler(IBreakdownRepository repository, IMapper mapper, IMediator mediator) : IRequestHandler<UpdateBreakdownCommand, BreakdownDTO>
    {
        private readonly IBreakdownRepository _repository = repository;
        private readonly IMapper _mapper = mapper;
        private readonly IMediator _mediator = mediator;
        public async Task<BreakdownDTO> Handle(UpdateBreakdownCommand request, CancellationToken cancellationToken)
        {
            var breakdown = _mapper.Map<BreakdownDTO>(request);
            await _repository.UpdateAsync(_mapper.Map<Breakdown>(breakdown));
            await _mediator.Publish(new BreakdownResponseNotification(_mapper.Map<List<BreakdownDTO>>(await _repository.GetAllAsync())), cancellationToken);
            return breakdown;
        }
    }
}
