using BgituSec.Application.DTOs;
using MediatR;

namespace BgituSec.Application.Features.Breakdowns.Commands
{
    public class DeleteBreakdownCommand : IRequest<BreakdownDTO>
    {
        public int Id { get; set; }
    }
}
