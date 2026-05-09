namespace CMMS.DAL.DTOs.Maps
{
    public class GeocodeResultDto
    {
        public string FormattedAddress { get; set; } = string.Empty;
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string? PlaceId { get; set; }
        public string? LocationType { get; set; }
    }
}
