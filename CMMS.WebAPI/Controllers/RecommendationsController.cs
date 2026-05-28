using CMMS.BLL.Interfaces;
using CMMS.DAL.DTOs.Recommendation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMMS.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RecommendationsController : ControllerBase
    {
        private readonly IRecommendationService _service;
        public RecommendationsController(IRecommendationService service) => _service = service;

        [Authorize(Roles = "Owner,Specialist")]
        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetListAsync());

        [Authorize(Roles = "Owner,Specialist")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetail(Guid id) => Ok(await _service.GetDetailAsync(id));

        [Authorize(Roles = "Owner,Specialist")]
        [HttpGet("diagnosis/{diagnosisId:guid}")]
        public async Task<IActionResult> GetByDiagnosis(Guid diagnosisId) => Ok(await _service.GetByDiagnosisIdAsync(diagnosisId));

        [Authorize(Roles = "Specialist")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RecommendationRequest req) => Ok(await _service.CreateAsync(req));

        [Authorize(Roles = "Specialist")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] RecommendationRequest req) => Ok(await _service.UpdateAsync(id, req));

        [Authorize(Roles = "Specialist")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id) => Ok(await _service.DeleteAsync(id));
    }
}
