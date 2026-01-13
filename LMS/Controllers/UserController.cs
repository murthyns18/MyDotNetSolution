using LMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;


namespace LMS.Controllers
{
    public class UserController : Controller
    {
        private static IList<User> users = new List<User>();

        private List<SelectListItem> GetRoles()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Text = "Admin", Value = "1" },
                new SelectListItem { Text = "Staff", Value = "2" },
                new SelectListItem { Text = "GeneralUser", Value = "3" }
            };
        }


        [HttpGet]
        public IActionResult AddUser()
        {
            User user = new User
            {
                RoleList = GetRoles()
            };

            return View(user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddUser(User model)
        {
            if (!ModelState.IsValid)
            {
               model.RoleList = GetRoles(); 
                return View(model);
            }

            model.UserID = users.Count == 0 ? 1 : users.Max(u => u.UserID) + 1;


            users.Add(model);
            return RedirectToAction("ListUser");
        }


        [HttpGet]
        public IActionResult ListUser()
        {
            return View(users);
        }

        [HttpGet]
        public IActionResult EditUser(int id)
        {
            var user = users.FirstOrDefault(u => u.UserID == id);
            if (user == null)
                return RedirectToAction("ListUser");

            user.RoleList = GetRoles();

            return View("AddUser", user);
        }

        [HttpPost]
        public IActionResult EditUser(User model)
        {
            if (!ModelState.IsValid)
            {
                model.RoleList = GetRoles();
                return View("AddUser", model);
            }

            var user = users.FirstOrDefault(u => u.UserID == model.UserID);

            if (user != null)
            {
                user.UserName = model.UserName;
                user.MobileNumber = model.MobileNumber;
                user.Email = model.Email;
                user.Address = model.Address;
                user.RoleID = model.RoleID;
                user.Status = model.Status;
            }

            return RedirectToAction("ListUser");
        }

        [HttpGet]
        public IActionResult ClearAll()
        {
            users.Clear();

            return RedirectToAction("ListUser");

        }

        [HttpPost]
        public IActionResult DeleteUser(int id)
        {
            var user = users.FirstOrDefault(u => u.UserID == id);
            if (user != null)
                users.Remove(user);

            return RedirectToAction("ListUser");

        }
    }
}