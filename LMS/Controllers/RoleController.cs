using LMS.Services;
using LMS_API.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LMS.Controllers
{
    [ServiceFilter(typeof(EncryptedActionParameterFilter))]
    public class RoleController : Controller
    {
        private readonly ILogger<RoleController> _logger;

        public RoleController(ILogger<RoleController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult AddRole()
        {
            try
            {
                return View(new Role());
            }
            catch (Exception ex)
            {
                SerilogErrorHelper.LogDetailedError(_logger, ex, HttpContext);
                TempData["Error"] = "Unable to load Add Role page.";
                return RedirectToAction("RoleList");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddRole(Role model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, error = ModelState.Values.SelectMany(v => v.Errors).First().ErrorMessage });
            }

            try
            {
                var result = API.Post("Role/SaveRole", HttpContext.Session.GetString("Token"), model);
                var message = JObject.Parse(result)["message"]?.ToString();
                return Json(new { success = true, message });
            }
            catch (Exception ex)
            {
                SerilogErrorHelper.LogDetailedError(_logger, ex, HttpContext);
                return Json(new { success = false, message = "Unable to save role." });
            }
        }

        [HttpGet]
        public IActionResult RoleList()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                SerilogErrorHelper.LogDetailedError(_logger, ex, HttpContext);
                TempData["Error"] = "Unable to load role list.";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public IActionResult EditRole(int roleID)
        {
            try
            {
                var role = JsonConvert.DeserializeObject<List<Role>>(API.Get("Role/GetRoles", HttpContext.Session.GetString("Token"), $"roleId={roleID}"))?.FirstOrDefault();
                if (role == null) return NotFound();
                return Json(role);
            }
            catch (Exception ex)
            {
                SerilogErrorHelper.LogDetailedError(_logger, ex, HttpContext);
                return StatusCode(500);
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
                return Json(new { success = true, message });
            }
            catch (Exception ex)
            {
                SerilogErrorHelper.LogDetailedError(_logger, ex, HttpContext);
                return Json(new { success = false, message = "Unable to delete role." });
            }
        }
    }
}
