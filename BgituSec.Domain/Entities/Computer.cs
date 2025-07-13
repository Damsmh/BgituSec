using NpgsqlTypes;

namespace BgituSec.Domain.Entities
{
    public class Computer
    {
        public int Id { get; set; }
        public NpgsqlPoint Position { get; set; }
        public string SerialNumber { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public int Type { get; set; }
        public int AuditoriumId { get; set; }
        public Auditorium Auditorium { get; set; }
        public ICollection<Breakdown> Breakdowns { get; set; }
    }
}
