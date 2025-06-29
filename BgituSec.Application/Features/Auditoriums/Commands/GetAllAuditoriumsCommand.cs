using BgituSec.Application.DTOs;
using MediatR;

namespace BgituSec.Application.Features.Auditoriums.Commands
{
    public class GetAllAuditoriumsCommand : IRequest<List<AuditoriumDTO>> { }
}
