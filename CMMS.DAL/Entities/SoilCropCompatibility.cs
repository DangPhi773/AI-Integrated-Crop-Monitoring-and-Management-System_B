using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.DAL.Entities
{
    public class SoilCropCompatibility
    {
        [Key]
        public Guid ComptId { get; set; }

        public Guid SoilId { get; set; }
        public virtual Soil Soil { get; set; } = null!;

        public Guid CropId { get; set; }
        public virtual Crop Crop { get; set; } = null!;

        public string? Compatibility { get; set; }
        public string? Note { get; set; }
    }
}
