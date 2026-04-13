using CMMS.BLL.Interfaces;
using CMMS.DAL.DTOs.Reports.Requests;
using CMMS.DAL.DTOs.Reports.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        [Authorize(Roles = "Owner,Worker,Specialist")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _reportService.GetAllReportsAsync();
            return Ok(result);
        }

        [Authorize(Roles = "Owner,Worker,Specialist")]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _reportService.GetReportByIdAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [Authorize(Roles = "Worker")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateReportRequest request, [FromForm] List<IFormFile>? images)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await _reportService.CreateReportAsync(request, userId, images);
            return result.Success ? StatusCode(201, result) : BadRequest(result);
        }

        [Authorize(Roles = "Owner")]
        [HttpPost("{reportId:guid}/assign")]
        public async Task<IActionResult> Assign(Guid reportId, [FromBody] AssignReportRequest request)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await _reportService.AssignReportAsync(reportId, request, userId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Specialist")]
        [HttpPost("{reportId:guid}/diagnosis")]
        public async Task<IActionResult> CreateDiagnosis(Guid reportId, [FromBody] CreateDiagnosisRequest request)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await _reportService.CreateDiagnosisAsync(reportId, request, userId);
            return result.Success ? StatusCode(201, result) : BadRequest(result);
        }

        [Authorize(Roles = "Owner,Specialist")]
        [HttpGet("diagnosis")]
        public async Task<IActionResult> GetAllDiagnosis()
        {
            var result = await _reportService.GetAllDiagnosisAsync();
            return Ok(result);
        }

        [Authorize(Roles = "Owner,Specialist")]
        [HttpGet("diagnosis/{diagnosisId:guid}")]
        public async Task<IActionResult> GetDiagnosisById(Guid diagnosisId)
        {
            var result = await _reportService.GetDiagnosisByIdAsync(diagnosisId);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [Authorize(Roles = "Owner,Worker,Specialist")]
        [HttpGet("{reportId:guid}/diagnosis")]
        public async Task<IActionResult> GetDiagnosisByReportId(Guid reportId)
        {
            var result = await _reportService.GetDiagnosisByReportIdAsync(reportId);
            return Ok(result);
        }

        [Authorize(Roles = "Owner")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _reportService.DeleteReportAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
