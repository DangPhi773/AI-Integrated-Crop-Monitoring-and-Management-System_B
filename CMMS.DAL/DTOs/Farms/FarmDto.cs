using System;
using System.ComponentModel.DataAnnotations;

namespace CMMS.DAL.DTOs.Farms
{
    public class FarmRequest
    {
        [StringLength(150, MinimumLength = 1, ErrorMessage = "Tên farm phải từ 1 đến 150 ký tự")]
        public string? FarmName { get; set; }

        [StringLength(500, ErrorMessage = "Địa chỉ tối đa 500 ký tự")]
        public string? FarmLocation { get; set; }

        [Range(0.0, 1000000, ErrorMessage = "Diện tích farm phải từ 0 đến 1,000,000")]
        public decimal? FarmArea { get; set; }

        [StringLength(50, ErrorMessage = "Status tối đa 50 ký tự")]
        public string? FarmStatus { get; set; }

        [Range(-90.0, 90.0, ErrorMessage = "Latitude phải từ -90 đến 90")]
        public decimal? Latitude { get; set; }

        [Range(-180.0, 180.0, ErrorMessage = "Longitude phải từ -180 đến 180")]
        public decimal? Longitude { get; set; }
    }

    public class FarmResponse
    {
        public Guid FarmId { get; set; }
        public string? FarmName { get; set; }
        public string? FarmLocation { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? FarmArea { get; set; }
        public string? FarmStatus { get; set; }
        public DateTime? FarmCreatedAt { get; set; }
        public int SeasonsCount { get; set; } = 0;
    }
}
