using CMMS.BLL.Interfaces;
using CMMS.DAL.DTOs.CropBedConfigs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMMS.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CropBedConfigsController : ControllerBase
    {
        private readonly ICropBedConfigService _service;
        public CropBedConfigsController(ICropBedConfigService service) => _service = service;

        [Authorize(Roles = "Owner,Worker")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var res = await _service.GetAllAsync();
            return res.Success ? Ok(res) : BadRequest(res);
        }

        [Authorize(Roles = "Owner,Worker")]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var res = await _service.GetByIdAsync(id);
            return res.Success ? Ok(res) : NotFound(res);
        }

        [Authorize(Roles = "Owner,Worker")]
        [HttpGet("crop/{cropId:guid}")]
        public async Task<IActionResult> GetByCrop(Guid cropId)
        {
            var res = await _service.GetByCropIdAsync(cropId);
            return res.Success ? Ok(res) : BadRequest(res);
        }

        [Authorize(Roles = "Owner")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CropBedConfigRequest request)
        {
            var res = await _service.CreateAsync(request);
            return res.Success ? Ok(res) : BadRequest(res);
        }

        [Authorize(Roles = "Owner")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CropBedConfigRequest request)
        {
            var res = await _service.UpdateAsync(id, request);
            return res.Success ? Ok(res) : BadRequest(res);
        }

        [Authorize(Roles = "Owner")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var res = await _service.DeleteAsync(id);
            return res.Success ? Ok(res) : BadRequest(res);
        }
    }
}
