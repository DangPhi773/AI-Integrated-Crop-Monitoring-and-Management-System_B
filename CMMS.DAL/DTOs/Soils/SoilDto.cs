using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.DAL.DTOs.Soils
{
    public class SoilRequest
    {
        public string Name { get; set; } = null!;
        public string? ScienceName { get; set; }
    }

    public class SoilResponse
    {
        public Guid SoilId { get; set; }
        public string Name { get; set; } = null!;
        public string? ScienceName { get; set; }

        public int CropsCount { get; set; } = 0;
        public int PlotsCount { get; set; } = 0;
    }
}
