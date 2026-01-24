using LMS.Models;
using LMS.Services;
using LMS_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;

namespace LMS.Controllers
{
    [ServiceFilter(typeof(EncryptedActionParameterFilter))]
    public class UserController : Controller
    {
        private readonly ILogger<BookController> _logger;

        public UserController(ILogger<BookController> logger)
        {
            _logger = logger;
        }
        private List<Role> LoadRoles()
        {
            try
            {
                var response = API.Get("Role/GetRoles", HttpContext.Session.GetString("Token"), "roleId=0");
                return JsonConvert.DeserializeObject<List<Role>>(response) ?? new List<Role>();
            }
            catch (Exception ex)
            {
                SerilogErrorHelper.LogDetailedError(_logger, ex, HttpContext);
                TempData["Error"] = "Unable to load roles.";
                return new List<Role>();
            }
        }

        private IEnumerable<SelectListItem> GetRoleSelectList()
        {
            return LoadRoles().Select(r => new SelectListItem { Text = r.RoleName, Value = r.RoleID.ToString() });
        }

        [HttpGet]
        public IActionResult AddUser()
        {
            try
            {
                var model = new User { RoleList = GetRoleSelectList() };
                return View(model);
            }
            catch (Exception ex)
            {
                SerilogErrorHelper.LogDetailedError(_logger, ex, HttpContext);
                TempData["Error"] = "Unable to load Add User page.";
                return RedirectToAction("ListUser");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddUser(User model)
        {
            model.RoleList = GetRoleSelectList();

            // For editing remove password validation in Edit
            if (model.UserID > 0)
            {
                ModelState.Remove(nameof(model.Password));
                ModelState.Remove(nameof(model.ConfirmPassword));
            }

            if (!ModelState.IsValid)
                return View(model);

            // Optional set pass = null
            if (model.UserID > 0 && string.IsNullOrWhiteSpace(model.Password))
            {
                model.Password = null;
            }

            try
            {
                var result = API.Post("User/SaveUser", HttpContext.Session.GetString("Token"), model);
                var message = JObject.Parse(result)["message"]?.ToString();

                if (message == "Email already exists.")
                {
                    ModelState.AddModelError("Email", message);
                    model.RoleList = GetRoleSelectList();
                    return View(model);
                }

                TempData["Message"] = message;
                return RedirectToAction("ListUser");
            }
            catch (Exception ex)
            {
                SerilogErrorHelper.LogDetailedError(_logger, ex, HttpContext);
                ModelState.AddModelError(string.Empty, "An error occurred while saving the user. Please try again.");
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult ListUser()
        {
            ViewBag.Roles = GetRoleSelectList();
            return View();
        }

        [HttpGet]
        public IActionResult EditUser(int userID)
        {
            var user = JsonConvert.DeserializeObject<List<User>>(
               API.Get("User/UserList", HttpContext.Session.GetString("Token"), $"userId={userID}")
               )?.FirstOrDefault();


            if (user == null)
                return NotFound();

            return Json(user);

        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                var result = API.Post($"User/DeleteUser?userID={id}", HttpContext.Session.GetString("Token"), new { });
                var message = JObject.Parse(result)["message"]?.ToString();
                TempData["Message"] = message;
            }
            catch (Exception ex)
            {
                SerilogErrorHelper.LogDetailedError(_logger, ex, HttpContext);
                TempData["Error"] = "Unable to delete user.";
            }

            return RedirectToAction("ListUser");
        }
    }
}
