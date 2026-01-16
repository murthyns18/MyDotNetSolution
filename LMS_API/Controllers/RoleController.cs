using LMS_API.Interfaces;
using LMS_API.Models;
using LMS_API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LMS_API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class RoleController : ControllerBase
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
                return Ok(_roleRepository.GetRoles(roleId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public IActionResult SaveRole(Role role)
        {
            try
            {
                var msg = _roleRepository.SaveRole(role);
                return Ok(new { message = msg });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPost]
        public IActionResult DeleteRole(int roleID)
        {
            var message = _roleRepository.DeleteRole(roleID);
            return Ok(new { message });
        }
    }
}
