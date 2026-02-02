using LMS_API.Interfaces;
using LMS_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace LMS_API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CategoryController : BaseController
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public IActionResult CategoryList(int categoryID)
        {
            try
            {
                IEnumerable<Category> list = _categoryRepository.GetList(categoryID);
                return Ok(list);
            }
            catch
            {
                return StatusCode(500, new { message = "Unable to fetch category list." });
            }
        }

        [HttpPost]
        public IActionResult SaveCategory(Category category)
        {
            if (!ModelState.IsValid) return BadRequest(new { message = "Invalid data." });

            try
            {
                string message = _categoryRepository.SaveCategory(category);
                return Ok(new { message });
            }
            catch
            {
                return StatusCode(500, new { message = "Unable to save category." });
            }
        }

        [HttpPost]
        public IActionResult DeleteCategory(int categoryID)
        {
            try
            {
                string message = _categoryRepository.DeleteCategory(categoryID);
                bool success = message.Contains("successfully");

                return success ? Ok(new { success = true, message }): BadRequest(new { success = false, message });
            }
            catch
            {
                return StatusCode(500, new { success = false, message = "Unable to delete category." });
            }
        }

    }
}
