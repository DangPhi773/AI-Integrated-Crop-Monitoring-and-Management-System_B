using CMMS.BLL.Interfaces;
using CMMS.DAL.DTOs.Crops;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMMS.WebAPI.Controllers
{
    [Authorize (Roles = "Owner")]
    [Route("api/[controller]")]
    [ApiController]
    public class CropsController : ControllerBase
    {
        private readonly ICropService _cropService;
        public CropsController(ICropService cropService) => _cropService = cropService;

        [Authorize(Roles = "Owner,Worker")]
        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _cropService.GetAllCropsAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id) => Ok(await _cropService.GetCropByIdAsync(id));

        [HttpPost]
        public async Task<IActionResult> Create(CropRequest request) => Ok(await _cropService.CreateCropAsync(request));

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, CropRequest request) => Ok(await _cropService.UpdateCropAsync(id, request));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id) => Ok(await _cropService.DeleteCropAsync(id));
    }
}
