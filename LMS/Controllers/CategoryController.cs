using LMS.Models;
using LMS.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

namespace LMS.Controllers
{
    public class CategoryController : Controller
    {

        
        [HttpGet]
        public IActionResult AddCategory()
        {
            return View(new Category());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddCategory(Category model)
        {
            if (!ModelState.IsValid)
                return View(model);

            API.Post("Category/SaveCategory", null, model);

            TempData["Message"] = model.CategoryID == 0
                ? "Category added successfully"
                : "Category updated successfully";

            return RedirectToAction("CategoryList");
        }

        [HttpGet]
        public IActionResult CategoryList()
        {
            var categories = JsonConvert.DeserializeObject<List<Category>>(
                API.Get("Category/CategoryList", null)
            ) ?? new List<Category>();

            return View(categories);
        }

        [HttpGet]
        public IActionResult EditCategory(int id)
        {
            var category = JsonConvert.DeserializeObject<List<Category>>(
                API.Get("Category/CategoryList", null, $"categoryId={id}")
            )?.FirstOrDefault();

            if (category == null)
                return RedirectToAction("CategoryList");

            return View("AddCategory", category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCategory(int categoryID)
        {
            //API.Post("Category/DeleteCategory", null, new { categoryID });
            API.Post($"Category/DeleteCategory?categoryID={categoryID}", null, new { });
       

            TempData["Message"] = "Category deleted successfully";
            return RedirectToAction("CategoryList");
        }

     
    }
}
