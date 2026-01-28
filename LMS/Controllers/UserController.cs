using LMS.Models;
using LMS.Services;
using LMS_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LMS.Controllers
{
    [ServiceFilter(typeof(EncryptedActionParameterFilter))]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        /* ================= ROLE LOAD ================= */

        private List<Role> LoadRoles()
        {
            try
            {
                var response = API.Get(
                    "Role/GetRoles",
                    HttpContext.Session.GetString("Token"),
                    "roleId=0"
                );

                return JsonConvert.DeserializeObject<List<Role>>(response)
                       ?? new List<Role>();
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
            return LoadRoles().Select(r => new SelectListItem
            {
                Text = r.RoleName,
                Value = r.RoleID.ToString()
            });
        }

        /* ================= ADD USER ================= */

        [HttpGet]
        public IActionResult AddUser()
        {
            try
            {
                return View(new User
                {
                    RoleList = GetRoleSelectList()
                });
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

            // Edit mode → Password optional
            if (model.UserID > 0)
            {
                ModelState.Remove(nameof(model.Password));
                ModelState.Remove(nameof(model.ConfirmPassword));
            }

            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    success = false,
                    errors = ModelState
                        .Where(x => x.Value.Errors.Count > 0)
                        .ToDictionary(
                            x => x.Key,
                            x => x.Value.Errors.First().ErrorMessage
                        )
                });
            }

            // Edit without password change
            if (model.UserID > 0 && string.IsNullOrWhiteSpace(model.Password))
            {
                model.Password = null;
            }

            try
            {
                var result = API.Post(
                    "User/SaveUser",
                    HttpContext.Session.GetString("Token"),
                    model
                );

                var message = JObject.Parse(result)["message"]?.ToString();

                if (message == "Email already exists.")
                {
                    return Json(new
                    {
                        success = false,
                        errors = new Dictionary<string, string>
                        {
                            { "Email", message }
                        }
                    });
                }

                // Fetch saved user for jqGrid update
                var savedUser = JsonConvert.DeserializeObject<List<User>>(
                    API.Get(
                        "User/UserList",
                        HttpContext.Session.GetString("Token"),
                        $"userId={model.UserID}"
                    )
                )?.FirstOrDefault();

                return Json(new
                {
                    success = true,
                    message,
                    data = savedUser
                });
            }
            catch (Exception ex)
            {
                SerilogErrorHelper.LogDetailedError(_logger, ex, HttpContext);
                return Json(new
                {
                    success = false,
                    errors = new Dictionary<string, string>
                    {
                        { "", "An error occurred while saving the user. Please try again." }
                    }
                });
            }
        }

        /* ================= LIST ================= */

        [HttpGet]
        public IActionResult ListUser()
        {
            try
            {
                ViewBag.Roles = GetRoleSelectList();
                return View();
            }
            catch (Exception ex)
            {
                SerilogErrorHelper.LogDetailedError(_logger, ex, HttpContext);
                TempData["Error"] = "Unable to load user list.";
                return RedirectToAction("Index", "Home");
            }
        }

        /* ================= EDIT ================= */

        [HttpGet]
        public IActionResult EditUser(int userID)
        {
            try
            {
                var user = JsonConvert.DeserializeObject<List<User>>(
                    API.Get(
                        "User/UserList",
                        HttpContext.Session.GetString("Token"),
                        $"userId={userID}"
                    )
                )?.FirstOrDefault();

                if (user == null)
                    return NotFound();

                return Json(user);
            }
            catch (Exception ex)
            {
                SerilogErrorHelper.LogDetailedError(_logger, ex, HttpContext);
                return StatusCode(500);
            }
        }

        /* ================= DELETE ================= */

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                var result = API.Post(
                    $"User/DeleteUser?userID={id}",
                    HttpContext.Session.GetString("Token"),
                    new { }
                );

                var message = JObject.Parse(result)["message"]?.ToString();

                return Json(new
                {
                    success = true,
                    message
                });
            }
            catch (Exception ex)
            {
                SerilogErrorHelper.LogDetailedError(_logger, ex, HttpContext);
                return Json(new
                {
                    success = false,
                    message = "Unable to delete user."
                });
            }
        }
    }
}
