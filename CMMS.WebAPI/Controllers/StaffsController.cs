using CMMS.BLL.Interfaces;
using CMMS.DAL.DTOs.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMMS.WebAPI.Controllers
{
    [Authorize(Roles = "Owner")] 
    [Route("api/[controller]")]
    [ApiController]
    public class StaffsController : ControllerBase
    {
        private readonly IStaffService _staffService;
        public StaffsController(IStaffService staffService) => _staffService = staffService;

        [HttpGet]
        public async Task<IActionResult> GetStaffs() => Ok(await _staffService.GetStaffListAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStaffDetail(Guid id) => Ok(await _staffService.GetStaffDetailAsync(id));

        [HttpPost]
        public async Task<IActionResult> CreateStaff([FromBody] StaffRequest request, [FromQuery] string role = "Worker")
        {
            var result = await _staffService.CreateStaffAsync(request, role);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPatch("{id}/assign-role")]
        public async Task<IActionResult> AssignRole(Guid id, [FromQuery] string roleName)
        {
            var result = await _staffService.AssignRoleAsync(id, roleName);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStaff(Guid id, [FromBody] StaffRequest request) => Ok(await _staffService.UpdateStaffAsync(id, request));

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStaff(Guid id) => Ok(await _staffService.RemoveStaffAsync(id));
    }
}