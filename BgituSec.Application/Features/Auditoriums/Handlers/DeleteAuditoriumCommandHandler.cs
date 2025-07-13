using AutoMapper;
using BgituSec.Application.DTOs;
using BgituSec.Application.Features.Auditoriums.Commands;
using BgituSec.Domain.Entities;
using BgituSec.Domain.Interfaces;
using MediatR;

namespace BgituSec.Application.Features.Auditoriums.Handlers
{
    public class DeleteAuditoriumCommandHandler(IAuditoriumRepository repository, IMapper mapper) : IRequestHandler<DeleteAuditoriumCommand, AuditoriumDTO>
    {
        private readonly IAuditoriumRepository _repository = repository;
        private readonly IMapper _mapper = mapper;
        public async Task<AuditoriumDTO> Handle(DeleteAuditoriumCommand request, CancellationToken cancellationToken)
        {
            Auditorium auditorium = await _repository.GetByIdAsync(request.Id);
            await _repository.DeleteAsync(auditorium.Id);
            return _mapper.Map<AuditoriumDTO>(auditorium);
        }
    }
}
