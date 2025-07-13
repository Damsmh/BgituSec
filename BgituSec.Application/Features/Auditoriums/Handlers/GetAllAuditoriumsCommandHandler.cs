using AutoMapper;
using BgituSec.Application.DTOs;
using BgituSec.Application.Features.Auditoriums.Commands;
using BgituSec.Domain.Interfaces;
using MediatR;

namespace BgituSec.Application.Features.Auditoriums.Handlers
{
    public class GetAllAuditoriumsCommandHandler(IAuditoriumRepository repository, IMapper mapper) : IRequestHandler<GetAllAuditoriumsCommand, List<AuditoriumDTO>>
    {
        private readonly IAuditoriumRepository _repository = repository;
        private readonly IMapper _mapper = mapper;
        public async Task<List<AuditoriumDTO>> Handle(GetAllAuditoriumsCommand request, CancellationToken cancellationToken)
        {
            var auditoriums = await _repository.GetAllAsync();
            List<AuditoriumDTO> mappedAuditoriums = [];
            foreach (var auditorium in auditoriums)
            {
                mappedAuditoriums.Add(_mapper.Map<AuditoriumDTO>(auditorium));
            }
            return mappedAuditoriums;
        }
    }
}
