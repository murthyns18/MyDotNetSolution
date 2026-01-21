using LMS.Models;
using LMS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LMS.Controllers
{
    [ServiceFilter(typeof(EncryptedActionParameterFilter))]
    public class PublisherController : Controller
    {
        private readonly ILogger<BookController> _logger;

        public PublisherController(ILogger<BookController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult AddPublisher()
        {
            try
            {
                return View(new Publisher());
            }
            catch (Exception ex)
            {
                SerilogErrorHelper.LogDetailedError(_logger, ex, HttpContext);
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
                var result = API.Post("Publisher/SavePublisher", HttpContext.Session.GetString("Token"), model);
                var message = JObject.Parse(result)["message"]?.ToString();
                TempData["Message"] = message;

                return RedirectToAction("PublisherList");
            }
            catch (Exception ex)
            {
                SerilogErrorHelper.LogDetailedError(_logger, ex, HttpContext);
                ModelState.AddModelError(string.Empty, "An error occurred while saving the publisher. Please try again.");
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult PublisherList()
        {
            //try
            //{
            //    var publishers = JsonConvert.DeserializeObject<List<Publisher>>(
            //        API.Get("Publisher/PublisherList", HttpContext.Session.GetString("Token"), "publisherID=0")
            //    ) ?? new List<Publisher>();

            return View();

            //}
            //catch
            //{
            //    TempData["Error"] = "Unable to load publisher list.";
            //    return View(new List<Publisher>());
            //}
        }

        [HttpGet]
        public IActionResult GetPublishersForGrid()
        {
            try
            {
                var publishers = JsonConvert.DeserializeObject<List<Publisher>>(
                    API.Get("Publisher/PublisherList", HttpContext.Session.GetString("Token"), "publisherID=0")
                ) ?? new List<Publisher>();

                return Json(new
                {
                    rows = publishers,
                    records = publishers.Count
                });
            }
            catch (Exception ex)
            {
                SerilogErrorHelper.LogDetailedError(_logger, ex, HttpContext);
                Response.StatusCode = 500;
                return Json(new
                {
                    error = "Failed to load publishers",
                    details = ex.Message
                });
            }
        }

        [HttpGet]
        public IActionResult EditPublisher(int publisherID)
        {
            try
            {
                var result = API.Get("Publisher/PublisherList", HttpContext.Session.GetString("Token"), $"publisherId={publisherID}");
                var publishers = JsonConvert.DeserializeObject<List<Publisher>>(result);
                var publisher = publishers?.FirstOrDefault();

                if (publisher == null)
                {
                    TempData["Error"] = "Publisher not found.";
                    return RedirectToAction("PublisherList");
                }

                return View("AddPublisher", publisher);
            }
            catch (Exception ex)
            {
                SerilogErrorHelper.LogDetailedError(_logger, ex, HttpContext);
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
                var result = API.Post("Publisher/DeletePublisher", HttpContext.Session.GetString("Token"), publisherID);
                var message = JObject.Parse(result)["message"]?.ToString();
                TempData["Message"] = message;
            }
            catch (Exception ex)
            {
                SerilogErrorHelper.LogDetailedError(_logger, ex, HttpContext);
                TempData["Error"] = "Unable to delete publisher.";
            }

            return RedirectToAction("PublisherList");
        }
    }
}
