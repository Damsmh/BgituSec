using BgituSec.Application.DTOs;
using MediatR;

namespace BgituSec.Application.Features.Buildings.Commands
{
    public class DeleteBuildingCommand : IRequest<BuildingDTO>
    {
        public int Id { get; set; }
    }
}
