using LMS.Services;
using LMS_API.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LMS.Controllers
{
    public class RoleController : Controller
    {
        [HttpGet]
        public IActionResult AddRole()
        {
            try
            {
                return View(new Role());
            }
            catch
            {
                TempData["Error"] = "Unable to load Add Role page.";
                return RedirectToAction("RoleList");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddRole(Role model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var result = API.Post("Role/SaveRole", HttpContext.Session.GetString("Token"), model);
                var message = JObject.Parse(result)["message"]?.ToString();
                TempData["Message"] = message;

                return RedirectToAction("RoleList");
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "An error occurred while saving the role. Please try again.");
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult RoleList()
        {
            try
            {
                var roles = JsonConvert.DeserializeObject<List<Role>>(API.Get("Role/GetRoles", HttpContext.Session.GetString("Token"))) ?? new List<Role>();
                return View(roles);
            }
            catch
            {
                TempData["Error"] = "Unable to load roles list.";
                return View(new List<Role>());
            }
        }

        [HttpGet]
        public IActionResult EditRole(short id)
        {
            try
            {
                var role = JsonConvert.DeserializeObject<List<Role>>(API.Get("Role/GetRoles", HttpContext.Session.GetString("Token"), $"roleId={id}"))?.FirstOrDefault();
                if (role == null)
                {
                    TempData["Error"] = "Role not found.";
                    return RedirectToAction("RoleList");
                }
                return View("AddRole", role);
            }
            catch
            {
                TempData["Error"] = "Unable to load role details.";
                return RedirectToAction("RoleList");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteRole(short roleID)
        {
            try
            {
                var result = API.Post($"Role/DeleteRole?RoleID={roleID}", HttpContext.Session.GetString("Token"), new { });
                var message = JObject.Parse(result)["message"]?.ToString();
                TempData["Message"] = message;
            }
            catch
            {
                TempData["Error"] = "Unable to delete role.";
            }
            return RedirectToAction("RoleList");
        }
    }
}
