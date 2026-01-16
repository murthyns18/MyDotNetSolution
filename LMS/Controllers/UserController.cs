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
            return JsonConvert.DeserializeObject<List<Role>>(
                API.Get("Role/GetRoles", null, "roleId=-1")
            ) ?? new List<Role>();
        }

        private IEnumerable<SelectListItem> GetRoleSelectList()
        {
            return LoadRoles().Select(r => new SelectListItem
            {
                Text = r.RoleName,
                Value = r.RoleID.ToString()
            });
        }

        [HttpGet]
        public IActionResult AddUser()
        {
            var model = new User
            {
                RoleList = GetRoleSelectList()
            };
            return View(model);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddUser(User model)
        {
            model.RoleList = GetRoleSelectList();

            if (!ModelState.IsValid)
                return View(model);

            API.Post("User/SaveUser", null, model);

            TempData["Message"] = model.UserID == 0
                ? "User added successfully"
                : "User updated successfully";

            return RedirectToAction("ListUser");
        }

        
        [HttpGet]
        public IActionResult ListUser()
        {
            var users = JsonConvert.DeserializeObject<List<User>>(
                API.Get("User/UserList", null, "userId=0")
            ) ?? new List<User>();

            var roles = LoadRoles();

            // Map RoleName
            foreach (var user in users)
            {
                user.RoleName = roles
                    .FirstOrDefault(r => r.RoleID == user.RoleID)
                    ?.RoleName;
            }

            return View(users);
        }


        [HttpGet]
        public IActionResult EditUser(int id)
        {
            var user = JsonConvert.DeserializeObject<List<User>>(
                API.Get("User/UserList", null, $"userId={id}")
            )?.FirstOrDefault();

            if (user == null)
                return RedirectToAction("ListUser");

            user.RoleList = GetRoleSelectList();
            return View("AddUser", user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteUser(int id)
        {
            var result = API.Post($"User/DeleteUser?userID={id}", null, null);
            TempData["Message"] = "User deleted successfully";
            return RedirectToAction("ListUser");
        }


    }
}
