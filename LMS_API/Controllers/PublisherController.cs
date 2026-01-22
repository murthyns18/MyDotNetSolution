using LMS_API.Interfaces;
using LMS_API.Models;
using LMS_API.Repositories;
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
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult SavePublisher(Publisher publisher)
        {
            try
            {
                var message = _publisherRepository.SavePublisher(publisher);
                return Ok(new { message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }


        [HttpPost]
        public IActionResult DeletePublisher([FromBody] int publisherID)
        {
            var message = _publisherRepository.DeletePublisher(publisherID);
            return Ok(new { message });
        }
    }
}
