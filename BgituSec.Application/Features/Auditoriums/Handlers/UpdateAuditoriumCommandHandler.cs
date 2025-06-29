using AutoMapper;
using BgituSec.Application.DTOs;
using BgituSec.Application.Features.Auditoriums.Commands;
using BgituSec.Domain.Entities;
using BgituSec.Domain.Interfaces;
using MediatR;

namespace BgituSec.Application.Features.Auditoriums.Handlers
{
    public class UpdateAuditoriumCommandHandler(IAuditoriumRepository repository, IMapper mapper) : IRequestHandler<UpdateAuditoriumCommand, AuditoriumDTO>
    {
        private readonly IAuditoriumRepository _repository = repository;
        private readonly IMapper _mapper = mapper;
        public async Task<AuditoriumDTO> Handle(UpdateAuditoriumCommand request, CancellationToken cancellationToken)
        {
            var auditorium = _mapper.Map<AuditoriumDTO>(request);
            await _repository.UpdateAsync(_mapper.Map<Auditorium>(auditorium));
            return auditorium;
        }
    }
}
