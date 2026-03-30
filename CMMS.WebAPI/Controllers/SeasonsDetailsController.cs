using CMMS.BLL.Interfaces;
using CMMS.DAL.DTOs.SeasonsDetails;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMMS.WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/seasons-details")]
    public class SeasonsDetailsController : ControllerBase
    {
        private readonly ISeasonsDetailService _seasonsDetailService;

        public SeasonsDetailsController(ISeasonsDetailService seasonsDetailService) => _seasonsDetailService = seasonsDetailService;

        [Authorize(Roles = "Owner,Worker")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _seasonsDetailService.GetAllSeasonsDetailsAsync();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Owner")]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _seasonsDetailService.GetSeasonsDetailByIdAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [Authorize(Roles = "Owner")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SeasonsDetailRequest request)
        {
            if (!ModelState.IsValid) return BadRequest("Invalid request");
            var result = await _seasonsDetailService.CreateSeasonsDetailAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Owner")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] SeasonsDetailRequest request)
        {
            var result = await _seasonsDetailService.UpdateSeasonsDetailAsync(id, request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Owner")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _seasonsDetailService.DeleteSeasonsDetailAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
