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
                var list = _categoryRepository.GetList(categoryID);
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
                var message = _categoryRepository.SaveCategory(category);
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
                var message = _categoryRepository.DeleteCategory(categoryID);
                return Ok(new { message });
            }
            catch
            {
                return StatusCode(500, new { message = "Unable to delete category." });
            }
        }
    }
}
