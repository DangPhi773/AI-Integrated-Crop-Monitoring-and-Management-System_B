using CMMS.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMMS.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/diagnosis-ai")]
public class DiagnosisAIController : ControllerBase
{
    private readonly IGeminiDiseaseService _geminiService;

    public DiagnosisAIController(IGeminiDiseaseService geminiService)
    {
        _geminiService = geminiService;
    }

    [HttpPost("identify")]
    public async Task<IActionResult> Identify(IFormFile image, [FromForm] string? organ = null)
    {
        if (image == null || image.Length == 0)
            return BadRequest(new { success = false, message = "Ảnh không hợp lệ" });

        var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp" };
        if (!allowedTypes.Contains(image.ContentType.ToLower()))
            return BadRequest(new { success = false, message = "Chỉ hỗ trợ ảnh JPEG, PNG, WebP" });

        if (image.Length > 10 * 1024 * 1024)
            return BadRequest(new { success = false, message = "Ảnh không được vượt quá 10MB" });

        var result = await _geminiService.AnalyzeImageAsync(image, organ);
        return Ok(new { success = true, data = result });
    }
}
