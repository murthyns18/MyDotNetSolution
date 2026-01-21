
using LMS.Models;
using LMS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace LMS.Controllers
{
    [ServiceFilter(typeof(EncryptedActionParameterFilter))]
    public class BookController : Controller
    {
        private readonly ILogger<BookController> _logger;

        public BookController(ILogger<BookController> logger)
        {
            _logger = logger;
        }

        private List<Publisher> LoadPublishers()
        {
            try
            {
                var response = API.Get("Publisher/PublisherList", HttpContext.Session.GetString("Token"), "PublisherID=0");
                return JsonConvert.DeserializeObject<List<Publisher>>(response) ?? new List<Publisher>();
            }
            catch(Exception ex)
            {
                SerilogErrorHelper.LogDetailedError(_logger, ex, HttpContext);
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
            catch (Exception ex)
            {
                SerilogErrorHelper.LogDetailedError(_logger, ex, HttpContext);
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
            catch (Exception ex)
            {
                SerilogErrorHelper.LogDetailedError(_logger, ex, HttpContext);
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
            catch (Exception ex)
            {
                SerilogErrorHelper.LogDetailedError(_logger, ex, HttpContext);
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

                return View();
            }
            catch (Exception ex)
            {
                SerilogErrorHelper.LogDetailedError(_logger, ex, HttpContext);
                TempData["Error"] = "Unable to load book list.";
                return View(new List<Book>());
            }
        }

        [HttpGet]
        public IActionResult GetBooksForGrid()
        {
            try
            {
                var books = JsonConvert.DeserializeObject<List<Book>>(
                    API.Get("Book/BookList",
                        HttpContext.Session.GetString("Token"),
                        "bookId=0")
                ) ?? new List<Book>();

                return Json(new
                {
                    rows = books,
                    records = books.Count
                });
            }
            catch (Exception ex)
            {
                SerilogErrorHelper.LogDetailedError(_logger, ex, HttpContext);
                Response.StatusCode = 500;
                return Json(new
                {
                    error = "Failed to load books",
                    details = ex.Message
                });
            }
        }

        [HttpGet]
        public IActionResult EditBook(int bookID)
        {
            try
            {

                var book = JsonConvert.DeserializeObject<List<Book>>(API.Get("Book/BookList", HttpContext.Session.GetString("Token"), $"bookId={bookID}"))?.FirstOrDefault();
                if (book == null)
                {
                    TempData["Error"] = "Book not found.";
                    return RedirectToAction("BookList");
                }
                book.PublisherList = GetPublisherSelectList();
                book.CategoryList = GetCategorySelectList();
                return View("AddBook", book);
            }
            catch (Exception ex)
            {
                SerilogErrorHelper.LogDetailedError(_logger, ex, HttpContext);
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
                var result = API.Post($"Book/DeleteBook", HttpContext.Session.GetString("Token"), bookID);
                var message = JObject.Parse(result)["message"]?.ToString();
                TempData["Message"] = message;
            }
            catch (Exception ex)
            {
                SerilogErrorHelper.LogDetailedError(_logger, ex, HttpContext);
                TempData["Error"] = "Unable to delete book.";
            }

            return RedirectToAction("BookList");
        }
    }
}
