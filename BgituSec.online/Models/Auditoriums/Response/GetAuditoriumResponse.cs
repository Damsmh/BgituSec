using NpgsqlTypes;

namespace BgituSec.Api.Models.Auditoriums.Response
{
    public class GetAuditoriumResponse
    {
        public int Id { get; set; }
        public int Floor { get; set; }
        public bool IsComputer { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string Size { get; set; }
        public int BuildingId { get; set; }
    }
}
