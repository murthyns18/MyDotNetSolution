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
            var list = _bookRepository.GetList(bookID);
            return Ok(list);
        }


        [HttpPost]
        public IActionResult SaveBook(Book book)
        {
            var message = _bookRepository.SaveBook(book);
            return Ok(new { message });
        }

        [HttpPost]  
        public IActionResult DeleteBook([FromBody] int bookID)
        {
            var message = _bookRepository.DeleteBook(bookID);
            return Ok(new { message });
        }

    }
}
