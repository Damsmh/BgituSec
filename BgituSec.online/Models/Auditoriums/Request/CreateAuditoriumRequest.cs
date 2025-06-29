using System.ComponentModel.DataAnnotations;

namespace BgituSec.Api.Models.Auditoriums.Request
{
    public class CreateAuditoriumRequest
    {
        [Required]
        public int Floor { get; set; }
        [Required]
        public bool IsComputer { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Position { get; set; }
        [Required]
        public string Size { get; set; }
        [Required]
        public int BuildingId { get; set; }
    }
}
