using LMS.Models;
using LMS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace LMS.Controllers
{
    public class BookController : Controller
    {
        /* ---------------- LOAD PUBLISHERS ---------------- */
        private List<Publisher> LoadPublishers()
        {
            return JsonConvert.DeserializeObject<List<Publisher>>(
                API.Get("Publisher/PublisherList", null)
            ) ?? new List<Publisher>();
        }

        private IEnumerable<SelectListItem> GetPublisherSelectList()
        {
            return LoadPublishers().Select(p => new SelectListItem
            {
                Text = p.PublisherName,
                Value = p.PublisherID.ToString()
            });
        }

        /* ---------------- LOAD CATEGORIES ---------------- */
        private List<Category> LoadCategories()
        {
            return JsonConvert.DeserializeObject<List<Category>>(
                API.Get("Category/CategoryList", null)
            ) ?? new List<Category>();
        }

        private IEnumerable<SelectListItem> GetCategorySelectList()
        {
            return LoadCategories().Select(c => new SelectListItem
            {
                Text = c.CategoryName,
                Value = c.CategoryID.ToString()
            });
        }

        /* ---------------- ADD BOOK (GET) ---------------- */
        [HttpGet]
        public IActionResult AddBook()
        {
            var model = new Book
            {
                PublisherList = GetPublisherSelectList(),
                CategoryList = GetCategorySelectList()
            };

            return View(model);
        }

        /* ---------------- ADD / UPDATE BOOK (POST) ---------------- */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddBook(Book model)
        {
            model.PublisherList = GetPublisherSelectList();
            model.CategoryList = GetCategorySelectList();

            if (!ModelState.IsValid)
                return View(model);

            API.Post("Book/SaveBook", null, model);

            TempData["Message"] = model.BookId == 0
                ? "Book added successfully"
                : "Book updated successfully";

            return RedirectToAction("BookList");
        }

        /* ---------------- BOOK LIST ---------------- */
        [HttpGet]
        public IActionResult BookList()
        {
            var books = JsonConvert.DeserializeObject<List<Book>>(
                API.Get("Book/BookList", null)
            ) ?? new List<Book>();

            var categories = LoadCategories();
            var publishers = LoadPublishers();

            // 🔥 MAP CATEGORY & PUBLISHER NAMES
            foreach (var book in books)
            {
                book.CategoryName = categories
                    .FirstOrDefault(c => c.CategoryID == book.CategoryID)
                    ?.CategoryName;

                book.PublisherName = publishers
                    .FirstOrDefault(p => p.PublisherID == book.PublisherID)
                    ?.PublisherName;
            }

            return View(books);
        }


        /* ---------------- EDIT BOOK ---------------- */
        [HttpGet]
        public IActionResult EditBook(int id)
        {
            var book = JsonConvert.DeserializeObject<List<Book>>(
                API.Get("Book/BookList", null, $"bookId={id}")
            )?.FirstOrDefault();

            if (book == null)
                return RedirectToAction("BookList");

            book.PublisherList = GetPublisherSelectList();
            book.CategoryList = GetCategorySelectList();

            return View("AddBook", book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteBook(int bookID)
        {
            API.Post("Book/DeleteBook", null, new { bookID = bookID });

            TempData["Message"] = "Book deleted successfully";
            return RedirectToAction("BookList");
        }

    }
}
