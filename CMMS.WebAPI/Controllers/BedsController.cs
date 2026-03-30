using CMMS.BLL.Interfaces;
using CMMS.DAL.DTOs.Beds;
using CMMS.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMMS.WebAPI.Controllers
{
    [Authorize (Roles = "Owner")]
    [ApiController]
    [Route("api/[controller]")]
    public class BedsController : ControllerBase
    {
        private readonly IBedService _bedService;

        public BedsController(IBedService bedService) => _bedService = bedService;

        [Authorize(Roles = "Owner,Worker")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _bedService.GetAllBedsAsync();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _bedService.GetBedByIdAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BedRequest request)
        {
            if (!ModelState.IsValid) return BadRequest("Invalid request");
            var result = await _bedService.CreateBedAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] BedRequest request)
        {
            var result = await _bedService.UpdateBedAsync(id, request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _bedService.DeleteBedAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
