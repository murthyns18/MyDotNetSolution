using LMS_API.Interfaces;
using LMS_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize] // Uncomment if authentication is required based on project standards
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentController(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        [HttpGet("GetList")]
        public IActionResult GetList(string? fiscalYear = null, string? quarter = null)
        {
            try
            {
                var payments = _paymentRepository.GetList(fiscalYear, quarter);
                return Ok(payments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("SavePayment")]
        public IActionResult SavePayment([FromBody] Payment payment)
        {
            try
            {
                var result = _paymentRepository.SavePayment(payment);
                return Ok(new { Message = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("DeletePayment/{id}")]
        public IActionResult DeletePayment(int id)
        {
            try
            {
                var result = _paymentRepository.DeletePayment(id);
                return Ok(new { Message = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("BulkUploadPayments")]
        public IActionResult BulkUploadPayments([FromBody] List<Payment> payments)
        {
            try
            {
                if (payments == null || !payments.Any())
                {
                    return BadRequest("No records provided.");
                }

                var result = _paymentRepository.BulkSavePayments(payments);
                return Ok(new { Message = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("OverwritePayments")]
        public IActionResult OverwritePayments([FromBody] List<Payment> payments)
        {
            try
            {
                if (payments == null || !payments.Any())
                {
                    return BadRequest("No records provided.");
                }

                var result = _paymentRepository.BulkUpdateOverrides(payments);
                return Ok(new { Message = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
