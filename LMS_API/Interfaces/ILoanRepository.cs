using LMS_API.Models;

namespace LMS_API.Interfaces
{
    public interface ILoanRepository
    {
        IEnumerable<LoanHeader> GetLoanList(int loanId = 0);
        IEnumerable<LoanDetail> GetLoanDetails(int loanId);
        string SaveLoan(LoanHeader loan);
        string ReturnLoan(int loanId);
    }
}
