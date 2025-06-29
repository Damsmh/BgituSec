using NpgsqlTypes;

namespace BgituSec.Api.Models.Auditoriums.Response
{
    public class GetAuditoriumResponse
    {
        public int Floor { get; set; }
        public bool IsComputer { get; set; }
        public string Name { get; set; }
        public NpgsqlPoint Position { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int BuildingId { get; set; }
    }
}
