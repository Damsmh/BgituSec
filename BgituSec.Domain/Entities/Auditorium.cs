using NpgsqlTypes;

namespace BgituSec.Domain.Entities
{
    public class Auditorium
    {
        public int Id { get; set; }
        public int Floor { get; set; }
        public bool IsComputer { get; set; }
        public string Name { get; set; }
        public NpgsqlPoint Position { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public int BuildingId { get; set; }

        public Building Building { get; set; }
        public ICollection<Computer> Computers { get; set; }
    }
}
