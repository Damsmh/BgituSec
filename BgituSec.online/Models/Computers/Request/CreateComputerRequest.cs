namespace BgituSec.Api.Models.Computers.Request
{
    public class CreateComputerRequest
    {
        public string Position { get; set; }
        public string SerialNumber { get; set; }
        public string Size { get; set; }
        public int AuditoriumId { get; set; }
    }
}
