using BgituSec.Application.DTOs;
using MediatR;

namespace BgituSec.Application.Features.Buildings.Commands
{
    public class CreateBuildingCommand : IRequest<BuildingDTO>
    {
        public int Number { get; set; }
        public int Floors { get; set; }
    }
}
