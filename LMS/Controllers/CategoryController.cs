using LMS.Models;
using LMS.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LMS.Controllers
{
    [ServiceFilter(typeof(EncryptedActionParameterFilter))]
    public class CategoryController : Controller
    {
        private readonly ILogger<BookController> _logger;

        public CategoryController(ILogger<BookController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult AddCategory()
        {
            try
            {
                return View(new Category());
            }
            catch (Exception ex)
            {
                SerilogErrorHelper.LogDetailedError(_logger, ex, HttpContext);
                TempData["Error"] = "Failed to load Add Category page.";
                return RedirectToAction("CategoryList");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddCategory(Category model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    success = false,
                    errors = ModelState.Where(x => x.Value.Errors.Count > 0).ToDictionary(x => x.Key, x => x.Value.Errors.First().ErrorMessage)
                });
            }

            try
            {
                var result = API.Post("Category/SaveCategory", HttpContext.Session.GetString("Token"), model);
                var message = JObject.Parse(result)["message"]?.ToString();
                return Json(new { success = true, message = message });
            }
            catch (Exception ex)
            {
                SerilogErrorHelper.LogDetailedError(_logger, ex, HttpContext);
                return Json(new { success = false, errors = new Dictionary<string, string> { { "", "Error occurred while saving category." } } });
            }
        }

        [HttpGet]
        public IActionResult CategoryList()
        {
            return View();
        }

        [HttpGet]
        public IActionResult EditCategory(int categoryID)
        {
            var category = JsonConvert.DeserializeObject<List<Category>>(API.Get("Category/CategoryList", HttpContext.Session.GetString("Token"), $"categoryID={categoryID}"))
                ?.FirstOrDefault();

            if (category == null) return NotFound();
            return Json(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCategory(int categoryID)
        {
            try
            {
                var result = API.Post($"Category/DeleteCategory?categoryID={categoryID}", HttpContext.Session.GetString("Token"), new { });
                var message = JObject.Parse(result)["message"]?.ToString();
                return Json(new { success = true, message });
            }
            catch (Exception ex)
            {
                SerilogErrorHelper.LogDetailedError(_logger, ex, HttpContext);
                return Json(new { success = false, message = "Unable to delete category." });
            }
        }
    }
}
