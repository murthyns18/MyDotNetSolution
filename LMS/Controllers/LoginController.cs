using LMS.Models;
using LMS.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Principal;

namespace LMS.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel loginViewModel)
        {
            var response = API.Post("Token", null, loginViewModel);

            if (string.IsNullOrEmpty(response))
            {
                TempData["Message"] = "Invalid email or password";
                TempData["Messageclass"] = "alert-danger";
                return View(loginViewModel);
            }

            var data = JsonConvert.DeserializeAnonymousType(response, new
            {
                user = new User(),
                access_token = string.Empty,
                menuDetails = new List<Menu>()
            });

            HttpContext.Session.SetString("Token", data.access_token);
            HttpContext.Session.SetString("MenuDetails", JsonConvert.SerializeObject(data.menuDetails));

            return RedirectToAction("BookList", "Book");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            // Clear all session data (Menu, Token, User info)
            HttpContext.Session.Clear();

            return RedirectToAction("Login", "Login");
        }
    }
}
