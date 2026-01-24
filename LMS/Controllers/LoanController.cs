using LMS.Models;
using LMS.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LMS.Controllers
{

    public class LoanController : Controller
    {
        
        [HttpGet]
        public IActionResult LoanList()
        {
            return View();
        }

       
        [HttpGet]
        public IActionResult AddLoan()
        {
            return View(new LoanHeader
            {
                LoanDetails = new List<LoanDetails>()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateLoan(LoanHeader loan)
        {
            // ✅ DEBUGGER WILL HIT HERE
            if (loan == null || loan.LoanDetails == null || !loan.LoanDetails.Any())
            {
                TempData["Error"] = "Please select at least one book";
                return RedirectToAction("AddLoan");
            }

            // force qty = 1
            loan.LoanDetails.ForEach(b => b.Qty = 1);

            var token = HttpContext.Session.GetString("Token");

            var response = API.Post(
                "Loan/CreateLoan",
                HttpContext.Session.GetString("Token"),
                loan
            );

            if (string.IsNullOrEmpty(response))
            {
                TempData["Error"] = "Loan creation failed";
                return RedirectToAction("AddLoan");
            }

            var message = JObject.Parse(response)["message"]?.ToString();
            TempData["Message"] = message;

            return RedirectToAction("LoanList");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteLoan(int loanId)
        {
            try
            {
                var result = API.Post(
                    "Loan/DeleteLoan",
                    HttpContext.Session.GetString("Token"),
                    loanId
                );

                var message = JObject.Parse(result)["message"]?.ToString();
                return Ok(new { message });
            }
            catch (Exception ex)
            {
                // SerilogErrorHelper.LogDetailedError(_logger, ex, HttpContext);
                return Ok(new { message = "Unable to delete loan." });
            }
        }



  
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ReturnLoan(int loanId)
        {
            var response = API.Post("Loan/ReturnLoan", null, loanId);

            var result = JsonConvert.DeserializeAnonymousType(response, new
            {
                message = string.Empty
            });

            return Ok(new { message = result.message });
        }
    }
}
