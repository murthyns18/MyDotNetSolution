using LMS_API.Models;
using LMS.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LMS.Controllers
{
    public class RoleController : Controller
    {
        [HttpGet]
        public IActionResult AddRole()
        {
            return View(new Role());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddRole(Role model)
        {
            if (!ModelState.IsValid)
                return View(model);

            API.Post("Role/SaveRole", null, model);

            TempData["Message"] = model.RoleID == 0
                ? "Role added successfully"
                : "Role updated successfully";

            return RedirectToAction("RoleList");
        }

        [HttpGet]
        public IActionResult RoleList()
        {
            var roles = JsonConvert.DeserializeObject<List<Role>>(
                API.Get("Role/GetRoles", null)
            ) ?? new List<Role>();

            return View(roles);
        }

        [HttpGet]
        public IActionResult EditRole(short id)
        {
            var role = JsonConvert.DeserializeObject<List<Role>>(
                API.Get("Role/GetRoles", null, $"roleId={id}")
            )?.FirstOrDefault();

            if (role == null)
                return RedirectToAction("RoleList");

            return View("AddRole", role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteRole(short roleID)
        {
            API.Post($"Role/DeleteRole?RoleID={roleID}", null, new { });

            TempData["Message"] = "Role deleted successfully";
            return RedirectToAction("RoleList");
        }
    }
}
