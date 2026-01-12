using LMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace LMS.Controllers
{
    public class BookController : Controller
    {
        private static IList<Book> books = new List<Book>();

        private List<SelectListItem> GetPublishers()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Text = "Penguin India",       Value = "1" },
                new SelectListItem { Text = "Rupa Publications",   Value = "2" },
                new SelectListItem { Text = "S. Chand Publishing", Value = "3" },
                new SelectListItem { Text = "Oxford India",        Value = "4" },
                new SelectListItem { Text = "Jaico Publishing",    Value = "5" }
            };
        }

        private string GetPublisherNameById(int id)
        {
            return GetPublishers().FirstOrDefault(p => p.Value == id.ToString())?.Text ?? "Unknown";
        }


        //Add book get
        [HttpGet]
        public IActionResult AddBook()
        {
            Book book = new Book
            {
                PublisherList = GetPublishers()
            };

            return View(book);
        }

        //Add book post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddBook(Book model)
        {
            //if (!ModelState.IsValid)
            //{
            //    model.PublisherList = GetPublishers();
            //    return View(model);
            //}

            model.Id = books.Count == 0 ? 1 : books.Max(b => b.Id) + 1;
            model.PublisherName = GetPublisherNameById(model.PublisherID);
            books.Add(model);

            return RedirectToAction("BookList");
        }

        //ListBook
        [HttpGet]
        public IActionResult BookList()
        {
            return View(books);
        }


        //Edit Book
        [HttpGet]
        public IActionResult EditBook(int id)
        {
            var book = books.FirstOrDefault(b => b.Id == id);
            if (book == null)
                return RedirectToAction("BookList");

            book.PublisherList = GetPublishers();

            return View("AddBook", book);
        }

        //Edit Book Post
        [HttpPost]
        public IActionResult EditBook(Book model)
        {
            //if (!ModelState.IsValid)
            //{
            //    model.PublisherList = GetPublishers();
            //    return View("AddBook", model);
            //}

            var book = books.FirstOrDefault(b => b.Id == model.Id);
            if (book != null)
            {
                book.Title = model.Title;
                book.Genre = model.Genre;
                book.PublisherID = model.PublisherID;
                book.PublisherName = GetPublisherNameById(model.PublisherID);
                book.Price = model.Price;
                book.Year = model.Year;
                book.Quantity = model.Quantity;
            }

            return RedirectToAction("BookList");
        }

        //Clear all
        [HttpGet]
        public IActionResult ClearAll()
        {
            books.Clear();
            return RedirectToAction("BookList");
        }

        
        //Delete book
        [HttpPost]
        public IActionResult DeleteBook(int id)
        {
            var book = books.FirstOrDefault(b => b.Id == id);
            if (book != null)
                books.Remove(book);

            return RedirectToAction("BookList");
        }
    }
}
