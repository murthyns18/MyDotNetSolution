namespace LMS_API.Models
{
    public class LoanHeader
    {
        public int LoanId { get; set; }
        public int UserId { get; set; }
        public int TotalQty { get; set; }

        public DateTime LoanDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }

        // For create loan (UI → API)
        public List<LoanDetails> LoanDetails { get; set; }
    }
}
