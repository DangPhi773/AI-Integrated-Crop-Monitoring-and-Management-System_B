using Microsoft.AspNetCore.Http;

namespace CMMS.WebAPI.Controllers
{
    public class PlantImageUploadRequestDto
    {
        public IFormFile Image { get; set; } = default!;
    }
}