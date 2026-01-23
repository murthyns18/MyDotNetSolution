namespace LMS.Models
{
    public class LoanDetails
    {
        public int LoanId { get; set; }
        public int BookId { get; set; }

        // Fixed internally (always 1)
        public int Qty { get; set; } = 1;
    }
}
