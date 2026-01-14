using LMS_API.Interfaces;
using LMS_API.Models;
using LMS_API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LMS_API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class BookController : Controller
    {
        private IBookRepository _bookRepository;

        public BookController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [HttpGet]
        public IActionResult BookList(int bookID)
        {
            try
            {
                var list = _bookRepository.GetList(bookID);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public IActionResult SaveBook(Book book)
        {
            try
            {
                var message = _bookRepository.SaveBook(book);
                return Ok(new { message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
