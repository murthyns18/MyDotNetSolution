using LMS.Models;
using LMS.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LMS.Controllers
{
    public class PublisherController : Controller
    {
        [HttpGet]
        public IActionResult AddPublisher()
        {
            try
            {
                return View(new Publisher());
            }
            catch
            {
                TempData["Error"] = "Unable to load Add Publisher page.";
                return RedirectToAction("PublisherList");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddPublisher(Publisher model)
        {
            if (!ModelState.IsValid)
                return View(model); 

            try
            {
                API.Post("Publisher/SavePublisher", null, model);
                TempData["Message"] = model.PublisherID == 0 ? "Publisher added successfully" : "Publisher updated successfully";
                return RedirectToAction("PublisherList");
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "An error occurred while saving the publisher. Please try again.");
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult PublisherList()
        {
            try
            {
                var publishers = JsonConvert.DeserializeObject<List<Publisher>>(API.Get("Publisher/PublisherList", null, "publisherID=0")) ?? new List<Publisher>();
                return View(publishers);
            }
            catch
            {
                TempData["Error"] = "Unable to load publisher list.";
                return View(new List<Publisher>());
            }
        }

        [HttpGet]
        public IActionResult EditPublisher(int id)
        {
            try
            {
                var publisher = JsonConvert.DeserializeObject<List<Publisher>>(API.Get("Publisher/PublisherList", null, $"publisherId={id}"))?.FirstOrDefault();
                if (publisher == null)
                {
                    TempData["Error"] = "Publisher not found.";
                    return RedirectToAction("PublisherList");
                }
                return View("AddPublisher", publisher);
            }
            catch
            {
                TempData["Error"] = "Unable to load publisher details.";
                return RedirectToAction("PublisherList");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePublisher(int publisherID)
        {
            try
            {
                var result = API.Post("Publisher/DeletePublisher", null, publisherID );
                var message = JObject.Parse(result)["message"]?.ToString();
                TempData["Message"] = message;
            }
            catch
            {
                TempData["Error"] = "Unable to delete publisher.";
            }
            return RedirectToAction("PublisherList");
        }
    }
}
