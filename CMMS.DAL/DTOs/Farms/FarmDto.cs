using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.DAL.DTOs.Farms
{
    public class FarmRequest
    {
        public string? FarmName { get; set; }
        public string? FarmLocation { get; set; }
        public decimal? FarmArea { get; set; }
        public string? FarmStatus { get; set; }

        public decimal? Latitude { get; set; }
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

        // counts / summary
        public int SeasonsCount { get; set; } = 0;
    }
}
