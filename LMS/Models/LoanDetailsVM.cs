
namespace LMS.Models
{
    public class LoanDetailsVM
    {
        public LoanHeader Header { get; set; } = new();
        public List<LoanDetail> Details { get; set; } = new();
    }

}
