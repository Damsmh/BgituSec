using AutoMapper;
using BgituSec.Application.DTOs;
using BgituSec.Application.Features.Buildings.Commands;
using BgituSec.Domain.Interfaces;
using MediatR;

namespace BgituSec.Application.Features.Buildings.Handlers
{
    public class DeleteBuildingCommandHandler(IBuildingRepository repository, IMapper mapper) : IRequestHandler<DeleteBuildingCommand, BuildingDTO>
    {
        private readonly IBuildingRepository _repository = repository;
        private readonly IMapper _mapper = mapper;
        public async Task<BuildingDTO> Handle(DeleteBuildingCommand request, CancellationToken cancellationToken)
        {
            var building = await _repository.GetByIdAsync(request.Id) ?? throw new KeyNotFoundException(nameof(request.Id));
            await _repository.DeleteAsync(request.Id);
            return _mapper.Map<BuildingDTO>(building);
        }
    }
}
