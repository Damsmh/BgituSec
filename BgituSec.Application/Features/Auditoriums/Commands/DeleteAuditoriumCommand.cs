using BgituSec.Application.DTOs;
using MediatR;

namespace BgituSec.Application.Features.Auditoriums.Commands
{
    public class DeleteAuditoriumCommand: IRequest<AuditoriumDTO>
    {
        public int Id { get; set; }
    }
}
