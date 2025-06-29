using System.ComponentModel.DataAnnotations;

namespace BgituSec.Api.Models.Buildings.Request
{
    public class BuildingRequest
    {
        [Required]
        public int Number { get; set; }
        [Required]
        public int Floors { get; set; }
    }
}
