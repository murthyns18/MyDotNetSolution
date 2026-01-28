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
            try
            {
                var list = _userRepository.GetList(userID);
                return Ok(list);
            }
            catch
            {
                return StatusCode(500, new { message = "Unable to fetch user list." });
            }
        }

        [HttpPost]
        public IActionResult SaveUser(User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid data." });

            try
            {
                var message = _userRepository.SaveUser(user);
                return Ok(new { message });
            }
            catch
            {
                return StatusCode(500, new { message = "Unable to save user." });
            }
        }

        [HttpPost]
        public IActionResult DeleteUser(int userID)
        {
            try
            {
                var message = _userRepository.DeleteUser(userID);
                return Ok(new { message });
            }
            catch
            {
                return StatusCode(500, new { message = "Unable to delete user." });
            }
        }

        [HttpGet]
        public IActionResult GetCountries()
        {
            try
            {
                var list = _userRepository.GetCountries();
                return Ok(list);
            }
            catch
            {
                return StatusCode(500, new { message = "Unable to fetch countries." });
            }
        }

        [HttpGet]
        public IActionResult GetStates(int countryId)
        {
            try
            {
                var list = _userRepository.GetStatesByCountry(countryId);
                return Ok(list);
            }
            catch
            {
                return StatusCode(500, new { message = "Unable to fetch states." });
            }
        }

        [HttpGet]
        public IActionResult GetCities(int stateId)
        {
            try
            {
                var list = _userRepository.GetCitiesByState(stateId);
                return Ok(list);
            }
            catch
            {
                return StatusCode(500, new { message = "Unable to fetch cities." });
            }
        }
    }
}
    