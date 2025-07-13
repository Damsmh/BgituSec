namespace BgituSec.Api.Models.Breakdowns.Response
{
    public class GetBreakdownResponse
    {
        public int id { get; set; }
        public string? description { get; set; }
        public bool isSolved { get; set; }
        public int level { get; set; }
        public string createdAt { get; set; }
        public int computerId { get; set; }
        public int userId { get; set; }
    }
}
