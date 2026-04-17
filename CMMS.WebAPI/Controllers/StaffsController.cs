using CMMS.BLL.Interfaces;
using CMMS.DAL.DTOs.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CMMS.WebAPI.Controllers
{
    [Authorize] 
    [Route("api/[controller]")]
    [ApiController]
    public class StaffsController : ControllerBase
    {
        private readonly IStaffService _staffService;
        public StaffsController(IStaffService staffService) => _staffService = staffService;

        [Authorize(Roles = "Owner")]
        [HttpGet]
        public async Task<IActionResult> GetStaffs() => Ok(await _staffService.GetStaffListAsync());

        [Authorize(Roles = "Owner")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStaffDetail(Guid id) => Ok(await _staffService.GetStaffDetailAsync(id));

        [Authorize(Roles = "Owner")]
        [HttpPost]
        public async Task<IActionResult> CreateStaff([FromBody] StaffRequest request, [FromQuery] string role = "Worker")
        {
            var result = await _staffService.CreateStaffAsync(request, role);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Owner")]
        [HttpPatch("{id}/assign-role")]
        public async Task<IActionResult> AssignRole(Guid id, [FromQuery] string roleName)
        {
            var result = await _staffService.AssignRoleAsync(id, roleName);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Owner")]
        [HttpGet("unassigned-role")]
        public async Task<IActionResult> GetUnassignedUsers()
        {
            var result = await _staffService.GetUsersWithoutRoleAsync();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Owner")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStaff(Guid id, [FromBody] StaffRequest request) => Ok(await _staffService.UpdateStaffAsync(id, request));

        [Authorize(Roles = "Owner")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStaff(Guid id) => Ok(await _staffService.RemoveStaffAsync(id));

        [HttpGet("me")]
        [Authorize] 
        public async Task<IActionResult> GetMyProfile()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();

            var result = await _staffService.GetMyProfileAsync(Guid.Parse(userIdClaim));
            return Ok(result);
        }
    }
}