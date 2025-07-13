using AutoMapper;
using BgituSec.Application.DTOs;
using BgituSec.Application.Features.Buildings.Commands;
using BgituSec.Domain.Entities;
using BgituSec.Domain.Interfaces;
using MediatR;

namespace BgituSec.Application.Features.Buildings.Handlers
{
    public class CreateBuildingCommandHandler(IBuildingRepository repository, IMapper mapper) : IRequestHandler<CreateBuildingCommand, BuildingDTO>
    {
        private readonly IBuildingRepository _repository = repository;
        private readonly IMapper _mapper = mapper;

        public async Task<BuildingDTO> Handle(CreateBuildingCommand request, CancellationToken cancellationToken)
        {
            Building building = _mapper.Map<Building>(request);
            await _repository.AddAsync(building);
            return _mapper.Map<BuildingDTO>(building);
        }
    }
}
