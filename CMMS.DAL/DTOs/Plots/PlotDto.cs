using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.DAL.DTOs.Plots
{
    public class PlotRequest
    {
        public Guid? FarmId { get; set; }
        public Guid? SoilId { get; set; }
        public string? PlotName { get; set; }
        public decimal? PlotArea { get; set; }
        public double? PlotLength { get; set; }
        public double? PlotWidth { get; set; }
        public double? PlotMarginLength { get; set; }
        public double? PlotMarginWidth { get; set; }
        public string? PlotStatus { get; set; }
    }

    public class PlotResponse
    {
        public Guid PlotId { get; set; }
        public Guid? FarmId { get; set; }
        public Guid? SoilId { get; set; }
        public string? PlotName { get; set; }
        public decimal? PlotArea { get; set; }
        public double? PlotLength { get; set; }
        public double? PlotWidth { get; set; }
        public double PlotMarginLength { get; set; }
        public double PlotMarginWidth { get; set; }
        public string? PlotStatus { get; set; }
        public DateTime? PlotCreatedAt { get; set; }

        public string? FarmName { get; set; }
        public string? SoilName { get; set; }
        public int BedsCount { get; set; } = 0;
    }
}
