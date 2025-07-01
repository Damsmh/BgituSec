using AutoMapper;
using BgituSec.Application.DTOs;
using BgituSec.Application.Features.Breakdowns.Commands;
using BgituSec.Domain.Interfaces;
using MediatR;

namespace BgituSec.Application.Features.Breakdowns.Handlers
{
    public class DeleteBreakdownCommandHandler(IBreakdownRepository repository, IMapper mapper) : IRequestHandler<DeleteBreakdownCommand, BreakdownDTO>
    {
        private readonly IBreakdownRepository _repository = repository;
        private readonly IMapper _mapper = mapper;
        public async Task<BreakdownDTO> Handle(DeleteBreakdownCommand request, CancellationToken cancellationToken)
        {
            var breakdown = _repository.DeleteAsync(request.Id);
            return _mapper.Map<BreakdownDTO>(breakdown);
        }
    }
}
