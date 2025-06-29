using AutoMapper;
using BgituSec.Application.DTOs;
using BgituSec.Application.Features.Buildings.Commands;
using BgituSec.Domain.Entities;
using BgituSec.Domain.Interfaces;
using MediatR;

namespace BgituSec.Application.Features.Buildings.Handlers
{
    public class GetAllBuuildingsCommandHandler(IBuildingRepository repository, IMapper mapper) : IRequestHandler<GetAllBuildingsCommand, List<BuildingDTO>>
    {
        private readonly IMapper _mapper = mapper;
        private readonly IBuildingRepository _repository = repository;

        public async Task<List<BuildingDTO>> Handle(GetAllBuildingsCommand request, CancellationToken cancellationToken)
        {
            var buildings =  await _repository.GetAllAsync();
            List<BuildingDTO> mappedBuildings = [];
            foreach (var building in buildings)
            {
                mappedBuildings.Add(_mapper.Map<BuildingDTO>(building));
            }
            return mappedBuildings;
        }
    }
}
