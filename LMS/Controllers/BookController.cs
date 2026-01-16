using LMS.Models;
using LMS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace LMS.Controllers
{
    public class BookController : Controller
    {

        //To load publishers from the api
        private List<Publisher> LoadPublishers()
        {
            return JsonConvert.DeserializeObject<List<Publisher>>(
                API.Get("Publisher/PublisherList", null)) ?? new List<Publisher>();
        }

        //using Loadpublishsers selecting SelectListItem
        private IEnumerable<SelectListItem> GetPublisherSelectList()
        {
            return LoadPublishers().Select(p => new SelectListItem
            {
                Text = p.PublisherName,
                Value = p.PublisherID.ToString()
            });
        }


        //Loading categories from the api
        private List<Category> LoadCategories()
        {
            return JsonConvert.DeserializeObject<List<Category>>(
                API.Get("Category/CategoryList", null)) ?? new List<Category>();
        }


        //using loadcategories selecting SelectListItem
        private IEnumerable<SelectListItem> GetCategorySelectList()
        {
            return LoadCategories().Select(c => new SelectListItem
            {
                Text = c.CategoryName,
                Value = c.CategoryID.ToString()
            });
        }



        //Add book GET
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


        //Add book POST
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


        //Book list GET
        [HttpGet]
        public IActionResult BookList()
        {
            var books = JsonConvert.DeserializeObject<List<Book>>(
                API.Get("Book/BookList", null)) ?? new List<Book>();

            var categories = LoadCategories();
            var publishers = LoadPublishers();


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


        //Edit book
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


        //Delete Book
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteBook(int bookID)
        {
            API.Post($"Book/DeleteBook?bookID={bookID}", null, new {});

            TempData["Message"] = "Book deleted successfully";
            return RedirectToAction("BookList");
        }

    }
}
