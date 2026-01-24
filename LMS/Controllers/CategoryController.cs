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
                return View(model);

            try
            {
                var result = API.Post("Category/SaveCategory", HttpContext.Session.GetString("Token"), model);
                var message = JObject.Parse(result)["message"]?.ToString();
                TempData["Message"] = message;

                return RedirectToAction("CategoryList");
            }
            catch (Exception ex)
            {
                SerilogErrorHelper.LogDetailedError(_logger, ex, HttpContext);
                ModelState.AddModelError(string.Empty, "Error occurred while saving category.");
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult CategoryList()
        {
            //try
            //{
            //    var categories = JsonConvert.DeserializeObject<List<Category>>(API.Get("Category/CategoryList", HttpContext.Session.GetString("Token"), "categoryID=0")) ?? new List<Category>();
                return View();
            //}
            //catch (Exception)
            //{
            //    TempData["Error"] = "Unable to load category list.";
            //    return View(new List<Category>());
            //}
        }



        [HttpGet]
        public IActionResult EditCategory(int categoryID)
        {
            var category = JsonConvert.DeserializeObject<List<Category>>(
                API.Get(
                    "Category/CategoryList",
                    HttpContext.Session.GetString("Token"),
                    $"categoryID={categoryID}"
                )
            )?.FirstOrDefault();

            if (category == null)
                return NotFound();

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
                TempData["Message"] = message;
            }
            catch (Exception ex)
            {
                SerilogErrorHelper.LogDetailedError(_logger, ex, HttpContext);
                TempData["Error"] = "Failed to delete category.";
            }
            return RedirectToAction("CategoryList");
        }
    }
}
