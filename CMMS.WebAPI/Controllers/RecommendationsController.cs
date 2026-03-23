using CMMS.BLL.Interfaces;
using CMMS.DAL.DTOs.Recommendation;
using Microsoft.AspNetCore.Mvc;

namespace CMMS.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecommendationsController : ControllerBase
    {
        private readonly IRecommendationService _service;
        public RecommendationsController(IRecommendationService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetListAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetail(Guid id) => Ok(await _service.GetDetailAsync(id));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RecommendationRequest req) => Ok(await _service.CreateAsync(req));

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] RecommendationRequest req) => Ok(await _service.UpdateAsync(id, req));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id) => Ok(await _service.DeleteAsync(id));
    }
}
