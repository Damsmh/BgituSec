namespace BgituSec.Domain.Entities
{
    public class Building
    {
        public int Id { get; set; }
        public int Floors { get; set; }
        public int Number { get; set; }
        public ICollection<Auditorium> Auditoriums { get; set; }
    }
}
