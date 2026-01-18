using LMS.Models;
using LMS.Services;
using LMS_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace LMS.Controllers
{
    public class UserController : Controller
    {
        private List<Role> LoadRoles()
        {
            try
            {
                var response = API.Get("Role/GetRoles", null, "roleId=0");
                return JsonConvert.DeserializeObject<List<Role>>(response) ?? new List<Role>();
            }
            catch
            {
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
            catch
            {
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
                API.Post("User/SaveUser", null, model);
                TempData["Message"] = model.UserID == 0 ? "User added successfully" : "User updated successfully";
                return RedirectToAction("ListUser");
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "An error occurred while saving the user. Please try again.");
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult ListUser()
        {
            try
            {
                var users = JsonConvert.DeserializeObject<List<User>>(API.Get("User/UserList", null, "userId=0")) ?? new List<User>();
                var roles = LoadRoles();

                foreach (var user in users)
                {
                    user.RoleName = roles.FirstOrDefault(r => r.RoleID == user.RoleID)?.RoleName;
                }

                return View(users);
            }
            catch
            {
                TempData["Error"] = "Unable to load users list.";
                return View(new List<User>());
            }
        }

        [HttpGet]
        public IActionResult EditUser(int id)
        {
            try
            {
                var user = JsonConvert.DeserializeObject<List<User>>(API.Get("User/UserList", null, $"userId={id}"))?.FirstOrDefault();
                if (user == null)
                {
                    TempData["Error"] = "User not found.";
                    return RedirectToAction("ListUser");
                }
                user.RoleList = GetRoleSelectList();
                return View("AddUser", user);
            }
            catch
            {
                TempData["Error"] = "Unable to load user details.";
                return RedirectToAction("ListUser");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                var result = API.Post($"User/DeleteUser?userID={id}", null, new { });
                TempData["Message"] = "User deleted successfully";
            }
            catch
            {
                TempData["Error"] = "Unable to delete user.";
            }
            return RedirectToAction("ListUser");
        }
    }
}
