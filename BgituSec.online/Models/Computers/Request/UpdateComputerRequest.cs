namespace BgituSec.Api.Models.Computers.Request
{
    public class UpdateComputerRequest
    {
        public int Id { get; set; }
        public string Position { get; set; }
        public string SerialNumber { get; set; }
        public string Size { get; set; }
        public int Type { get; set; }
        public int AuditoriumId { get; set; }
    }
}
