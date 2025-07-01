using BgituSec.Application.DTOs;
using MediatR;
using NpgsqlTypes;

namespace BgituSec.Application.Features.Computers.Commands
{
    public class CreateComputerCommand : IRequest<ComputerDTO>
    {
        public NpgsqlPoint Position { get; set; }
        public string SerialNumber { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public int AuditoriumId { get; set; }
    }
}
