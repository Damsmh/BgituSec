using AutoMapper;
using BgituSec.Application.DTOs;
using BgituSec.Application.Features.Breakdowns.Commands;
using BgituSec.Domain.Interfaces;
using MediatR;

namespace BgituSec.Application.Features.Breakdowns.Handlers
{
    public class DeleteBreakdownCommandHandler(IBreakdownRepository repository, IMapper mapper, IMediator mediator) : IRequestHandler<DeleteBreakdownCommand, BreakdownDTO>
    {
        private readonly IBreakdownRepository _repository = repository;
        private readonly IMapper _mapper = mapper;
        private readonly IMediator _mediator = mediator;
        public async Task<BreakdownDTO> Handle(DeleteBreakdownCommand request, CancellationToken cancellationToken)
        {
            var breakdown = _repository.DeleteAsync(request.Id);
            await _mediator.Publish(new BreakdownResponseNotification(_mapper.Map<List<BreakdownDTO>>(await _repository.GetAllAsync())), cancellationToken);
            return _mapper.Map<BreakdownDTO>(breakdown);
        }
    }
}
