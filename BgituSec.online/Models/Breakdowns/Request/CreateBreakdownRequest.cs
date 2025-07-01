namespace BgituSec.Api.Models.Breakdowns.Request
{
    public class CreateBreakdownRequest
    {
        public string? Description { get; set; }
        public bool IsSolved { get; set; }
        public int Level { get; set; }
        public int ComputerId { get; set; }
        public int UserId { get; set; }
    }
}
