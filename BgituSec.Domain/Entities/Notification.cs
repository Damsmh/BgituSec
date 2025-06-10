namespace BgituSec.Domain.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public bool IsViewed { get; set; }
        public int BreakdownId { get; set; }
        public int UserId { get; set; }

        public Breakdown Breakdown { get; set; }
        public User User { get; set; }
    }
}
