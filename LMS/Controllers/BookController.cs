
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

                if (message == "Book already exists with ISBN")
                {
                    ModelState.AddModelError("ISBN", message);
                    model.PublisherList = GetPublisherSelectList();
                    model.CategoryList = GetCategorySelectList();
                    return View(model);
                }
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
            ViewBag.Publishers = GetPublisherSelectList();
            ViewBag.Categories = GetCategorySelectList();
            return View();
        }


        [HttpGet]
        public IActionResult EditBook(int bookID)
        {
            var book = JsonConvert.DeserializeObject<List<Book>>(
                API.Get("Book/BookList", HttpContext.Session.GetString("Token"), $"bookId={bookID}")
            )?.FirstOrDefault();

            if (book == null)
                return NotFound();

            return Json(book);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteBook([FromForm] int bookID)
        {
            try
            {
                var result = API.Post(
                    "Book/DeleteBook",
                    HttpContext.Session.GetString("Token"),
                    bookID
                );

                var message = JObject.Parse(result)["message"]?.ToString();
                return Json(new { success = true, message });
            }
            catch (Exception ex)
            {
                SerilogErrorHelper.LogDetailedError(_logger, ex, HttpContext);
                return Json(new { success = false });
            }
        }

    }
}