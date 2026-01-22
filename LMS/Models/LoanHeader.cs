namespace LMS.Models
{
    public class LoanHeader
    {
        public int LoanId { get; set; }

        public int BorrowerId { get; set; }

        // 🔴 THIS MUST MATCH API JSON EXACTLY
        public string BorrowerName { get; set; } = string.Empty;

        public DateTime LoanDate { get; set; }

        // 🔴 MUST NOT BE NON-NULLABLE
        public DateTime? DueDate { get; set; }

        public DateTime? ReturnDate { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
