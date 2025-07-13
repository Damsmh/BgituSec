using BgituSec.Application.DTOs;
using MediatR;

namespace BgituSec.Application.Features.Buildings.Commands
{
    public class GetAllBuildingsCommand : IRequest<List<BuildingDTO>> { }
}
