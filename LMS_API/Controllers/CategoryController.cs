using LMS_API.Interfaces;
using LMS_API.Models;
using LMS_API.Repositories;
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
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public IActionResult SaveCategory(Category category)
        {
            try
            {
                var message = _categoryRepository.SaveCategory(category);
                return Ok(new { message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



        [HttpPost]
        public IActionResult DeleteCategory(int categoryID)
        {
            var message = _categoryRepository.DeleteCategory(categoryID);
            return Ok(new { message });
        }
    }
}
