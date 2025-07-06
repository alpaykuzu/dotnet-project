using API.DTOs.UserRole;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {
        private readonly IUserRoleService _roleService;
        public UserRoleController(IUserRoleService userRoleService)
        {
            _roleService = userRoleService;
        }

        [HttpPut("update-user-role")]
        public async Task<IActionResult> UpdateUserRole(UserRoleRequest userRoleRequest)
        {
            return Ok(await _roleService.UpdateUserRoleAsync(userRoleRequest));
        }

    }
}
