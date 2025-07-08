namespace BgituSec.Application.DTOs
{
    public class BreakdownDTO
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Description { get; set; }
        public bool IsSolved { get; set; }
        public int Level { get; set; }
        public int ComputerId { get; set; }
        public int UserId { get; set; }
    }
}
