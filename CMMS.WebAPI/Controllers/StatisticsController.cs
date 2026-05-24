using CMMS.BLL.Interfaces;
using CMMS.DAL.DTOs.Statistics.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMMS.WebAPI.Controllers
{
    [ApiController]
    [Route("api/statistics")]
    [Authorize(Roles = "Owner,Specialist")]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsService _service;
        public StatisticsController(IStatisticsService service) => _service = service;

        [HttpGet("yield/by-crop")]
        public async Task<IActionResult> GetYieldByCrop([FromQuery] YieldStatsFilter filter)
        {
            var result = await _service.GetYieldByCropAsync(filter);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("yield/by-season")]
        public async Task<IActionResult> GetYieldBySeason([FromQuery] YieldStatsFilter filter)
        {
            var result = await _service.GetYieldBySeasonAsync(filter);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("yield/by-plot")]
        public async Task<IActionResult> GetYieldByPlot([FromQuery] YieldStatsFilter filter)
        {
            var result = await _service.GetYieldByPlotAsync(filter);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("yield/summary")]
        public async Task<IActionResult> GetYieldSummary([FromQuery] YieldStatsFilter filter)
        {
            var result = await _service.GetYieldSummaryAsync(filter);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
