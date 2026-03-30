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

        [Authorize(Roles = "Owner,Worker")]
        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _cropService.GetAllCropsAsync());

        [Authorize(Roles = "Owner")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id) => Ok(await _cropService.GetCropByIdAsync(id));

        [Authorize(Roles = "Owner")]
        [HttpPost]
        public async Task<IActionResult> Create(CropRequest request) => Ok(await _cropService.CreateCropAsync(request));

        [Authorize(Roles = "Owner")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, CropRequest request) => Ok(await _cropService.UpdateCropAsync(id, request));

        [Authorize(Roles = "Owner")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id) => Ok(await _cropService.DeleteCropAsync(id));
    }
}
