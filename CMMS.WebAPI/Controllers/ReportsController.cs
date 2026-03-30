using CMMS.BLL.Interfaces;
using CMMS.DAL.DTOs.Reports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMMS.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [Authorize(Roles = "Owner,Worker")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _reportService.GetAllReportsAsync();
            return Ok(result);
        }

        [Authorize(Roles = "Owner,Worker")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _reportService.GetReportByIdAsync(id);
            if (!result.Success) return NotFound(result);
            return Ok(result);
        }

        [Authorize(Roles = "Owner,Worker")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ReportRequest request)
        {
            var result = await _reportService.CreateReportAsync(request);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [Authorize(Roles = "Owner,Worker")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ReportRequest request)
        {
            var result = await _reportService.UpdateReportAsync(id, request);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [Authorize(Roles = "Owner,Worker")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _reportService.DeleteReportAsync(id);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }
    }
}