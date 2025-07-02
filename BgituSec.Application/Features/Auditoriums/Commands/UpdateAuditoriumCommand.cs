using BgituSec.Application.DTOs;
using MediatR;
using NpgsqlTypes;

namespace BgituSec.Application.Features.Auditoriums.Commands
{
    public class UpdateAuditoriumCommand : IRequest<AuditoriumDTO>
    {
        public int Id { get; set; }
        public int Floor { get; set; }
        public bool IsComputer { get; set; }
        public string Name { get; set; }
        public NpgsqlPoint Position { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public int BuildingId { get; set; }
    }
}
