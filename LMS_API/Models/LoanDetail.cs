namespace LMS_API.Models
{
    public class LoanDetail
    {
        public int LoanDetailId { get; set; }
        public int LoanId { get; set; }
        public int BookId { get; set; }
        public string? BookTitle { get; set; }
    }
}
