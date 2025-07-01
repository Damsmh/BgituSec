namespace BgituSec.Api.Models.Computers.Response
{
    public class GetComputerResponse
    {
        public int Id { get; set; }
        public string Position { get; set; }
        public string SerialNumber { get; set; }
        public string Size { get; set; }
        public int AuditoriumId { get; set; }
    }
}
