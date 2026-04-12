using CMMS.DAL.DTOs.Crops;
using CMMS.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.BLL.Mappings
{
    public static class SoilCropCompatibilityMapper
    {
        public static SoilCropCompatibilityResponse ToResponse(SoilCropCompatibility x) => new()
        {
            ComptId = x.ComptId,
            SoilId = x.SoilId,
            SoilName = x.Soil?.Name,
            CropId = x.CropId,
            CropName = x.Crop?.CropName,
            Compatibility = x.Compatibility,
            Note = x.Note
        };

        public static IEnumerable<SoilCropCompatibilityResponse> ToResponseList(IEnumerable<SoilCropCompatibility> list)
            => list.Select(ToResponse);
    }
}
