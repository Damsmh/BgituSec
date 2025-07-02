using NpgsqlTypes;

namespace BgituSec.Application.DTOs
{
    public class AuditoriumDTO
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
