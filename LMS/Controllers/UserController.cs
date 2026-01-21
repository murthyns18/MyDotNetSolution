using LMS.Models;
using LMS.Services;
using LMS_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
            //try
            //{
            //    var users = JsonConvert.DeserializeObject<List<User>>(
            //        API.Get("User/UserList", HttpContext.Session.GetString("Token"), "userId=0")
            //    ) ?? new List<User>();

            //    var roles = LoadRoles();

            //    foreach (var user in users)
            //    {
            //        user.RoleName = roles.FirstOrDefault(r => r.RoleID == user.RoleID)?.RoleName;
            //    }

            //    return View(users);
            //}
            //catch
            //{
            //    TempData["Error"] = "Unable to load users list.";
            //    return View(new List<User>());
            //}

            return View();
        }

        [HttpGet]
        public IActionResult GetUsersForGrid()
        {
            try
            {
                var users = JsonConvert.DeserializeObject<List<User>>(
                    API.Get("User/UserList", HttpContext.Session.GetString("Token"), "userId=0")
                ) ?? new List<User>();

                var roles = LoadRoles();

                foreach (var user in users)
                {
                    user.RoleName = roles.FirstOrDefault(r => r.RoleID == user.RoleID)?.RoleName;
                }

                return Json(new
                {
                    rows = users,
                    records = users.Count
                });
            }
            catch (Exception ex)
            {
                SerilogErrorHelper.LogDetailedError(_logger, ex, HttpContext);
                Response.StatusCode = 500;
                return Json(new
                {
                    error = "Failed to load users",
                    details = ex.Message
                });
            }
        }

        [HttpGet]
        public IActionResult EditUser(int userID)
        {
            try
            {
                var result = API.Get("User/UserList", HttpContext.Session.GetString("Token"), $"userId={userID}");
                var users = JsonConvert.DeserializeObject<List<User>>(result);
                var user = users?.FirstOrDefault();

                if (user == null)
                {
                    TempData["Error"] = "User not found.";
                    return RedirectToAction("ListUser");
                }

                user.RoleList = GetRoleSelectList();
                return View("AddUser", user);
            }
            catch (Exception ex)
            {
                SerilogErrorHelper.LogDetailedError(_logger, ex, HttpContext);
                TempData["Error"] = "Unable to load user details.";
                return RedirectToAction("ListUser");
            }
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
