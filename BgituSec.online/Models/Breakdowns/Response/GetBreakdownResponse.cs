namespace BgituSec.Api.Models.Breakdowns.Response
{
    public class GetBreakdownResponse
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public bool IsSolved { get; set; }
        public int Level { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ComputerId { get; set; }
        public int UserId { get; set; }
    }
}
