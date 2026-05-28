using System.ComponentModel.DataAnnotations;

namespace CMMS.DAL.DTOs.Maps
{
    public class UpdateFarmCoordinatesRequestDto
    {
        [StringLength(500, ErrorMessage = "Address tối đa 500 ký tự")]
        public string? Address { get; set; }

        [Range(-90.0, 90.0, ErrorMessage = "Latitude phải từ -90 đến 90")]
        public decimal? Latitude { get; set; }

        [Range(-180.0, 180.0, ErrorMessage = "Longitude phải từ -180 đến 180")]
        public decimal? Longitude { get; set; }
    }
}
