using LMS_API.Interfaces;
using LMS_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace LMS_API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : BaseController
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult UserList(int userID)
        {
            var list = _userRepository.GetList(userID);
            return Ok(list);
        }

        [HttpPost]
        public IActionResult SaveUser(User user)
        {
            var message = _userRepository.SaveUser(user);
            return Ok(new { message });
        }

        [HttpPost]
        public IActionResult DeleteUser(int userID)
        {
            var message = _userRepository.DeleteUser(userID);
            return Ok(new { message });
        }
    }
}
