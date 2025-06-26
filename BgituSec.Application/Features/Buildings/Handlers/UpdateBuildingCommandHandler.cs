using AutoMapper;
using BgituSec.Application.DTOs;
using BgituSec.Application.Features.Buildings.Commands;
using BgituSec.Domain.Interfaces;
using MediatR;

namespace BgituSec.Application.Features.Buildings.Handlers
{
    public class UpdateBuildingCommandHandler(IBuildingRepository repository, IMapper mapper) : IRequestHandler<UpdateBuildingCommand, BuildingDTO>
    {
        private readonly IBuildingRepository _repository = repository;
        private readonly IMapper _mapper = mapper;

        public async Task<BuildingDTO> Handle(UpdateBuildingCommand request, CancellationToken cancellationToken)
        {
            var building = await _repository.GetByNumber(request.Number) ?? throw new KeyNotFoundException(nameof(request.Number));
            await _repository.UpdateAsync(building);
            return _mapper.Map<BuildingDTO>(building);
        }
    }
}
