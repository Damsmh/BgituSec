using NpgsqlTypes;

namespace BgituSec.Application.DTOs
{
    public class ComputerDTO
    {
        public int Id { get; set; }
        public NpgsqlPoint Position { get; set; }
        public string SerialNumber { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public int Type { get; set; }
        public int AuditoriumId { get; set; }
    }
}
