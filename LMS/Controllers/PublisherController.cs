using LMS.Models;
using LMS.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LMS.Controllers
{
    public class PublisherController : Controller
    {
        [HttpGet]
        public IActionResult AddPublisher()
        {
            return View(new Publisher());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddPublisher(Publisher model)
        {
            if (!ModelState.IsValid)
                return View(model);

            API.Post("Publisher/SavePublisher", null, model);

            TempData["Message"] = model.PublisherID == 0
                ? "Publisher added successfully"
                : "Publisher updated successfully";

            return RedirectToAction("PublisherList");
        }

        [HttpGet]
        public IActionResult PublisherList()
        {
            var publishers = JsonConvert.DeserializeObject<List<Publisher>>(
                API.Get("Publisher/PublisherList", null)
            ) ?? new List<Publisher>();

            return View(publishers);
        }

        [HttpGet]
        public IActionResult EditPublisher(int id)
        {
            var publisher = JsonConvert.DeserializeObject<List<Publisher>>(
                API.Get("Publisher/PublisherList", null, $"publisherId={id}")
            )?.FirstOrDefault();

            if (publisher == null)
                return RedirectToAction("PublisherList");

            return View("AddPublisher", publisher);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePublisher(int publisherID)
        {
            API.Post("Publisher/DeletePublisher", null, new { publisherID });

            TempData["Message"] = "Publisher deleted successfully";
            return RedirectToAction("PublisherList");
        }


        [HttpGet]
        public IActionResult ClearAll()
        {
            API.Post("Publisher/ClearAll", null, null);

            TempData["Message"] = "All publishers cleared successfully";
            return RedirectToAction("PublisherList");
        }
    }
}
