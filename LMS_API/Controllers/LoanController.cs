using LMS_API.Interfaces;
using LMS_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace LMS_API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class LoanController : ControllerBase
    {
        private readonly ILoanRepository _loanRepository;

        public LoanController(ILoanRepository loanRepository)
        {
            _loanRepository = loanRepository;
        }

        [HttpGet]
        public IActionResult LoanList(int loanId = 0)
        {
            var list = _loanRepository.GetLoanList(loanId);
            return Ok(list);
        }

        [HttpGet]
        public IActionResult LoanDetails(int loanId)
        {
            var list = _loanRepository.GetLoanDetails(loanId);
            return Ok(list);
        }

        [HttpPost]
        public IActionResult SaveLoan(LoanHeader loan)
        {
            var message = _loanRepository.SaveLoan(loan);
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
