using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.DAL.Entities
{
    public class IotDevice
    {
        public Guid DeviceId { get; set; } = Guid.NewGuid();
        public Guid? BedId { get; set; }
        public string Name { get; set; } = null!;
        public string? Type { get; set; }
        public string? Status { get; set; }
        public DateTime? InstallationDate { get; set; }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public virtual Bed? Bed { get; set; }
        public virtual ICollection<IotData> IotDatas { get; set; } = new List<IotData>();
    }
}
