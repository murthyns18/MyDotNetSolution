using LMS.Models;
using LMS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LMS.Controllers
{
    public class BookController : Controller
    {
        private List<Publisher> LoadPublishers()
        {
            try
            {
                var response = API.Get("Publisher/PublisherList", HttpContext.Session.GetString("Token"), "PublisherID=0");
                return JsonConvert.DeserializeObject<List<Publisher>>(response) ?? new List<Publisher>();
            }
            catch
            {
                TempData["Error"] = "Unable to load publishers.";
                return new List<Publisher>();
            }
        }

        private IEnumerable<SelectListItem> GetPublisherSelectList()
        {
            return LoadPublishers().Select(p => new SelectListItem { Text = p.PublisherName, Value = p.PublisherID.ToString() });
        }

        private List<Category> LoadCategories()
        {
            try
            {
                var response = API.Get("Category/CategoryList", HttpContext.Session.GetString("Token"), "categoryID=0");
                return JsonConvert.DeserializeObject<List<Category>>(response) ?? new List<Category>();
            }
            catch
            {
                TempData["Error"] = "Unable to load categories.";
                return new List<Category>();
            }
        }

        private IEnumerable<SelectListItem> GetCategorySelectList()
        {
            return LoadCategories().Select(c => new SelectListItem { Text = c.CategoryName, Value = c.CategoryID.ToString() });
        }

        [HttpGet]
        public IActionResult AddBook()
        {
            try
            {
                var model = new Book { PublisherList = GetPublisherSelectList(), CategoryList = GetCategorySelectList() };
                return View(model);
            }
            catch
            {
                TempData["Error"] = "Unable to load Add Book page.";
                return RedirectToAction("BookList");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddBook(Book model)
        {
            model.PublisherList = GetPublisherSelectList();
            model.CategoryList = GetCategorySelectList();

            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var result = API.Post("Book/SaveBook", HttpContext.Session.GetString("Token"), model);
                var message = JObject.Parse(result)["message"]?.ToString();
                TempData["Message"] = message;
              
                return RedirectToAction("BookList");
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "An error occurred while saving the book. Please try again.");
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult BookList()
        {
            try
            {
                var books = JsonConvert.DeserializeObject<List<Book>>(API.Get("Book/BookList", HttpContext.Session.GetString("Token"), "bookID=0")) ?? new List<Book>();
                var categories = LoadCategories();
                var publishers = LoadPublishers();

                foreach (var book in books)
                {
                    book.CategoryName = categories.FirstOrDefault(c => c.CategoryID == book.CategoryID)?.CategoryName;
                    book.PublisherName = publishers.FirstOrDefault(p => p.PublisherID == book.PublisherID)?.PublisherName;
                }

                return View(books);
            }
            catch
            {
                TempData["Error"] = "Unable to load book list.";
                return View(new List<Book>());
            }
        }

        [HttpGet]
        public IActionResult EditBook(int id)
        {
            try
            {
                var book = JsonConvert.DeserializeObject<List<Book>>(API.Get("Book/BookList", HttpContext.Session.GetString("Token"), $"bookId={id}"))?.FirstOrDefault();
                if (book == null)
                {
                    TempData["Error"] = "Book not found.";
                    return RedirectToAction("BookList");
                }
                book.PublisherList = GetPublisherSelectList();
                book.CategoryList = GetCategorySelectList();
                return View("AddBook", book);
            }
            catch
            {
                TempData["Error"] = "Unable to load book details.";
                return RedirectToAction("BookList");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteBook(int bookID)
        {
            try
            {
                var result = API.Post($"Book/DeleteBook?bookID={bookID}", HttpContext.Session.GetString("Token"), new { });
                var message = JObject.Parse(result)["message"]?.ToString();
                TempData["Message"] = message;
            }
            catch
            {
                TempData["Error"] = "Unable to delete book.";
            }

            return RedirectToAction("BookList");
        }
    }
}
