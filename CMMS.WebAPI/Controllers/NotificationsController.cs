using CMMS.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CMMS.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _service;

        public NotificationsController(INotificationService service)
        {
            _service = service;
        }

        private Guid CurrentUserId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpGet]
        public async Task<IActionResult> GetMine([FromQuery] bool unreadOnly = false, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var result = await _service.GetMyNotificationsAsync(CurrentUserId, unreadOnly, page, pageSize);
            return Ok(result);
        }

        [HttpGet("unread-count")]
        public async Task<IActionResult> UnreadCount()
        {
            var result = await _service.GetUnreadCountAsync(CurrentUserId);
            return Ok(result);
        }

        [HttpPut("{id:guid}/read")]
        public async Task<IActionResult> MarkRead(Guid id)
        {
            var result = await _service.MarkAsReadAsync(id, CurrentUserId);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpPut("read-all")]
        public async Task<IActionResult> MarkAllRead()
        {
            var result = await _service.MarkAllAsReadAsync(CurrentUserId);
            return Ok(result);
        }
    }
}
