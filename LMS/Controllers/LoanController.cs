using LMS.Models;
using LMS.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LMS.Controllers
{
    public class LoanController : Controller
    {
        private readonly ILogger<LoanController> _logger;

        public LoanController(ILogger<LoanController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult LoanList()
        {
            return View();
        }

        // jqGrid data
        [HttpGet]
        public IActionResult GetLoansForGrid()
        {
            var response = API.Get(
                "Loan/LoanList",
                HttpContext.Session.GetString("Token"),
                "loanId=0"
            );

            var loans = JsonConvert.DeserializeObject<List<LoanHeader>>(response) ?? new();

            return Json(new
            {
                rows = loans,
                records = loans.Count
            });
        }

        // Modal details
        [HttpGet]
        public IActionResult LoanDetails(int loanId)
        {
            var headerResponse = API.Get(
                "Loan/LoanList",
                HttpContext.Session.GetString("Token"),
                $"loanId={loanId}"
            );

            var header = JsonConvert
                .DeserializeObject<List<LoanHeader>>(headerResponse)?
                .FirstOrDefault();

            var detailResponse = API.Get(
                "Loan/LoanDetails",
                HttpContext.Session.GetString("Token"),
                $"loanId={loanId}"
            );

            var details = JsonConvert
                .DeserializeObject<List<LoanDetail>>(detailResponse) ?? new();

            return PartialView("_LoanDetails", new LoanDetailsVM
            {
                Header = header!,
                Details = details
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ReturnLoan(int loanId)
        {
            var result = API.Post(
                "Loan/ReturnLoan",
                HttpContext.Session.GetString("Token"),
                loanId
            );

            TempData["Message"] = JsonConvert.DeserializeObject<dynamic>(result)?.message;
            return RedirectToAction("LoanList");
        }
    }
}
