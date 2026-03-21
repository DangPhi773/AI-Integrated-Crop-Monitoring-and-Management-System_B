using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.DAL.Entities
{
    public class IotData
    {
        public Guid SensorDataId { get; set; } = Guid.NewGuid();
        public Guid? DeviceId { get; set; }
        public Guid? SeasonId { get; set; }

        public DateTime? RecordedAt { get; set; }
        public string? Type { get; set; }
        public double? Value { get; set; }
        public string? Unit { get; set; }
        public bool? IsAlert { get; set; }
        public double? Min { get; set; }
        public double? Max { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual IotDevice? Device { get; set; }
    }
}
