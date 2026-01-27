using LMS_API.Interfaces;
using LMS_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace LMS_API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PublisherController : BaseController
    {
        private readonly IPublisherRepository _publisherRepository;

        public PublisherController(IPublisherRepository publisherRepository)
        {
            _publisherRepository = publisherRepository;
        }

        [HttpGet]
        public IActionResult PublisherList(int publisherID)
        {
            try
            {
                var list = _publisherRepository.GetList(publisherID);
                return Ok(list);
            }
            catch
            {
                return StatusCode(500, new { message = "Unable to fetch publisher list." });
            }
        }

        [HttpPost]
        public IActionResult SavePublisher(Publisher publisher)
        {
            if (!ModelState.IsValid) return BadRequest(new { message = "Invalid data." });

            try
            {
                var message = _publisherRepository.SavePublisher(publisher);
                return Ok(new { message });
            }
            catch
            {
                return StatusCode(500, new { message = "Unable to save publisher." });
            }
        }

        [HttpPost]
        public IActionResult DeletePublisher([FromBody] int publisherID)
        {
            try
            {
                var message = _publisherRepository.DeletePublisher(publisherID);
                return Ok(new { message });
            }
            catch
            {
                return StatusCode(500, new { message = "Unable to delete publisher." });
            }
        }
    }
}
