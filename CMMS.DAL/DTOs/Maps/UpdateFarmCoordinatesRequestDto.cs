namespace CMMS.DAL.DTOs.Maps
{
    public class UpdateFarmCoordinatesRequestDto
    {
        public string? Address { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
    }
}
