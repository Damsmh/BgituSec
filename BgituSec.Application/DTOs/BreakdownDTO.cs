namespace BgituSec.Application.DTOs
{
    public class BreakdownDTO
    {
        public int id { get; set; }
        public DateTime createdAt { get; set; }
        public string? description { get; set; }
        public bool isSolved { get; set; }
        public int level { get; set; }
        public int computerId { get; set; }
        public int userId { get; set; }
    }
}
