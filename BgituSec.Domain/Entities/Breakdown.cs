namespace BgituSec.Domain.Entities
{
    public class Breakdown
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Description { get; set; }
        public bool IsSolved { get; set; }
        public int Level { get; set; }
        public int ComputerId { get; set; }
        public int UserId { get; set; }

        public Computer Computer { get; set; }
        public User User { get; set; }


    }
}
