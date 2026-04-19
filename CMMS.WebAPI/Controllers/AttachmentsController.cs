using CMMS.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CMMS.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AttachmentsController : ControllerBase
    {
        private readonly IAttachmentService _attachmentService;

        public AttachmentsController(IAttachmentService attachmentService)
        {
            _attachmentService = attachmentService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(
            IFormFile file,
            [FromForm] string objectType,
            [FromForm] Guid objectId,
            [FromForm] string attachmentType,
            [FromForm] string? description = null)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await _attachmentService.UploadAsync(file, objectType, objectId, attachmentType, userId, description);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetByObject([FromQuery] string objectType, [FromQuery] Guid objectId)
        {
            var result = await _attachmentService.GetByObjectAsync(objectType, objectId);
            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _attachmentService.SoftDeleteAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
