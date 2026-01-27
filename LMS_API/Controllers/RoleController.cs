using LMS_API.Interfaces;
using LMS_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace LMS_API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class RoleController : BaseController
    {
        private readonly IRoleRepository _roleRepository;

        public RoleController(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        [HttpGet]
        public IActionResult GetRoles(short roleId = 0)
        {
            try
            {
                var list = _roleRepository.GetRoles(roleId);
                return Ok(list);
            }
            catch
            {
                return StatusCode(500, new { message = "Unable to fetch roles." });
            }
        }

        [HttpPost]
        public IActionResult SaveRole(Role role)
        {
            if (!ModelState.IsValid) return BadRequest(new { message = "Invalid data." });

            try
            {
                var message = _roleRepository.SaveRole(role);
                return Ok(new { message });
            }
            catch
            {
                return StatusCode(500, new { message = "Unable to save role." });
            }
        }

        [HttpPost]
        public IActionResult DeleteRole(int roleID)
        {
            try
            {
                var message = _roleRepository.DeleteRole(roleID);
                return Ok(new { message });
            }
            catch
            {
                return StatusCode(500, new { message = "Unable to delete role." });
            }
        }
    }
}
