using AutoMapper;
using BgituSec.Application.DTOs;
using BgituSec.Application.Features.Auditoriums.Commands;
using BgituSec.Domain.Entities;
using BgituSec.Domain.Interfaces;
using MediatR;

namespace BgituSec.Application.Features.Auditoriums.Handlers
{
    public class CreateAuditoriumCommandHandler(IAuditoriumRepository repository, IMapper mapper) : IRequestHandler<CreateAuditoriumCommand, AuditoriumDTO>
    {
        private readonly IAuditoriumRepository _repository = repository;
        private readonly IMapper _mapper = mapper;
        public async Task<AuditoriumDTO> Handle(CreateAuditoriumCommand request, CancellationToken cancellationToken)
        {
            Auditorium auditorium =_mapper.Map<Auditorium>(request);
            await _repository.AddAsync(auditorium);
            return _mapper.Map<AuditoriumDTO>(auditorium);
        }
    }
}
