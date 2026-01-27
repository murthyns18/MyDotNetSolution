using LMS_API.Interfaces;
using LMS_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace LMS_API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class BookController : BaseController
    {
        private readonly IBookRepository _bookRepository;

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
            catch
            {
                return StatusCode(500, new { message = "Unable to fetch book list." });
            }
        }

        [HttpGet]
        public IActionResult GetBooksByPublisher(int publisherId)
        {
            try
            {
                var list = _bookRepository.GetByPublisher(publisherId);
                return Ok(list);
            }
            catch
            {
                return StatusCode(500, new { message = "Unable to fetch books by publisher." });
            }
        }

        [HttpPost]
        public IActionResult SaveBook(Book book)
        {
            if (!ModelState.IsValid) return BadRequest(new { message = "Invalid data." });

            try
            {
                var message = _bookRepository.SaveBook(book);
                return Ok(new { message });
            }
            catch
            {
                return StatusCode(500, new { message = "Unable to save book." });
            }
        }

        [HttpPost]
        public IActionResult DeleteBook([FromBody] int bookID)
        {
            try
            {
                var message = _bookRepository.DeleteBook(bookID);
                return Ok(new { message });
            }
            catch
            {
                return StatusCode(500, new { message = "Unable to delete book." });
            }
        }
    }
}
