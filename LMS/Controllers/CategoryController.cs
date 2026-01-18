using LMS.Models;
using LMS.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LMS.Controllers
{
    public class CategoryController : Controller
    {
        [HttpGet]
        public IActionResult AddCategory()
        {
            try
            {
                return View(new Category());
            }
            catch (Exception)
            {
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
                API.Post("Category/SaveCategory", null, model);
                TempData["Message"] = model.CategoryID == 0 ? "Category added successfully" : "Category updated successfully";
                return RedirectToAction("CategoryList");
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Error occurred while saving category.");
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult CategoryList()
        {
            try
            {
                var categories = JsonConvert.DeserializeObject<List<Category>>(API.Get("Category/CategoryList", HttpContext.Session.GetString("Token"), "categoryID=0")) ?? new List<Category>();
                return View(categories);
            }
            catch (Exception)
            {
                TempData["Error"] = "Unable to load category list.";
                return View(new List<Category>());
            }
        }

        [HttpGet]
        public IActionResult EditCategory(int id)
        {
            try
            {
                var category = JsonConvert.DeserializeObject<List<Category>>(API.Get("Category/CategoryList", null, $"categoryId={id}"))?.FirstOrDefault();
                if (category == null)
                {
                    TempData["Error"] = "Category not found.";
                    return RedirectToAction("CategoryList");
                }
                return View("AddCategory", category);
            }
            catch (Exception)
            {
                TempData["Error"] = "Unable to load category details.";
                return RedirectToAction("CategoryList");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCategory(int categoryID)
        {
            try
            {
                var result = API.Post($"Category/DeleteCategory?categoryID={categoryID}", null, new { });
                var message = JObject.Parse(result)["message"]?.ToString();
                TempData["Message"] = message ?? "Category deleted successfully";
            }
            catch (Exception)
            {
                TempData["Error"] = "Failed to delete category.";
            }
            return RedirectToAction("CategoryList");
        }
    }
}
