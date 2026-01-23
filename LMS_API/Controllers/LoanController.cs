using LMS_API.Interfaces;
using LMS_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace LMS_API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class LoanController : BaseController
    {
        private readonly ILoanRepository _loanRepository;

        public LoanController(ILoanRepository loanRepository)
        {
            _loanRepository = loanRepository;
        }

        [HttpGet]
        public IActionResult LoanList(int loanId)
        {
            var list = _loanRepository.GetList(loanId);
            return Ok(list);
        }

        [HttpPost]
        public IActionResult CreateLoan(LoanHeader loan)
        {
            var message = _loanRepository.CreateLoan(loan);
            return Ok(new { message });
        }

        [HttpPost]
        public IActionResult DeleteLoan([FromBody] int loanId)
        {
            var message = _loanRepository.DeleteLoan(loanId);
            return Ok(new { message });
        }

        [HttpPost]
        public IActionResult ReturnLoan([FromBody] int loanId)
        {
            var message = _loanRepository.ReturnLoan(loanId);
            return Ok(new { message });
        }
    }
}
