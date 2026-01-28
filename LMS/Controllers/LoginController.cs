using LMS.Models;
using LMS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace LMS.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<BookController> _logger;

        public LoginController(ILogger<BookController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                SerilogErrorHelper.LogDetailedError(_logger, ex, HttpContext);
                return View();
            }
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel loginViewModel)
        {
            try
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
            catch (Exception ex)
            {
                SerilogErrorHelper.LogDetailedError(_logger, ex, HttpContext);
                TempData["Message"] = ex;
                TempData["Messageclass"] = "alert-danger";
                return View(loginViewModel);
            }
        }

        [HttpGet]
        public IActionResult Logout()
        {
            try
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Login", "Login");
            }
            catch (Exception ex)
            {
                SerilogErrorHelper.LogDetailedError(_logger, ex, HttpContext);
                return RedirectToAction("Login", "Login");
            }
        }
    }
}
