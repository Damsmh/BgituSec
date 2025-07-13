using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace BgituSec.Domain.Entities
{
    [Index(nameof(Number), Name = "IX_Number", IsUnique = false)]
    public class Building
    {
        public int Id { get; set; }
        public int Floors { get; set; }
        public int Number { get; set; }
        public ICollection<Auditorium> Auditoriums { get; set; }
    }
}
