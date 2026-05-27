using CMMS.BLL.Interfaces;
using CMMS.DAL.DTOs.Crops;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMMS.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CropsController : ControllerBase
    {
        private readonly ICropService _cropService;
        public CropsController(ICropService cropService) => _cropService = cropService;

        [Authorize(Roles = "Owner,Worker,Specialist")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _cropService.GetAllCropsAsync();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Owner,Specialist")]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _cropService.GetCropByIdAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [Authorize(Roles = "Owner")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CropRequest request)
        {
            var result = await _cropService.CreateCropAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Owner")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CropRequest request)
        {
            var result = await _cropService.UpdateCropAsync(id, request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Owner")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _cropService.DeleteCropAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
