namespace LMS.Models
{
    public class LoanHeader
    {
        public int LoanId { get; set; }
        public int UserId { get; set; }
        public int TotalQty { get; set; }

        public DateTime LoanDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        // For create loan
        public List<LoanDetails> LoanDetails { get; set; } = new();
    }
}
