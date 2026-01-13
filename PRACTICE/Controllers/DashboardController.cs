using Microsoft.AspNetCore.Mvc;
using PRACTICE.Filters;

namespace PRACTICE.Controllers
{
    [ServiceFilter(typeof(AuthFilter))]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
