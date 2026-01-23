using LMS.Models;
using LMS.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LMS.Controllers
{
    public class LoanController : Controller
    {
        // -----------------------------
        // 1️⃣ Loan List Page
        // -----------------------------
        [HttpGet]
        public IActionResult LoanList()
        {
            return View();
        }

        // -----------------------------
        // 2️⃣ Add Loan Page
        // -----------------------------
        [HttpGet]
        public IActionResult AddLoan()
        {
            return View(new LoanHeader
            {
                LoanDetails = new List<LoanDetails>()
            });
        }

        // -----------------------------
        // 3️⃣ Create Loan
        // -----------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateLoan(LoanHeader loan)
        {
            // UI-level safety (DB enforces rules anyway)
            if (loan == null || loan.LoanDetails == null || !loan.LoanDetails.Any())
            {
                return Ok(new { message = "Please select at least one book" });
            }

            if (loan.LoanDetails.Count > 4)
            {
                return Ok(new { message = "You can borrow a maximum of 4 books only" });
            }

            // Ensure Qty is always 1 (ignore UI)
            loan.LoanDetails.ForEach(b => b.Qty = 1);

            var response = API.Post("Loan/CreateLoan", null, loan);

            if (string.IsNullOrEmpty(response))
            {
                return Ok(new { message = "Failed to create loan" });
            }

            var result = JsonConvert.DeserializeAnonymousType(response, new
            {
                message = string.Empty
            });

            return Ok(new { message = result.message });
        }

        // -----------------------------
        // 4️⃣ Delete Loan
        // -----------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteLoan(int loanId)
        {
            var response = API.Post("Loan/DeleteLoan", null, loanId);

            var result = JsonConvert.DeserializeAnonymousType(response, new
            {
                message = string.Empty
            });

            return Ok(new { message = result.message });
        }

        // -----------------------------
        // 5️⃣ Return Loan
        // -----------------------------
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
