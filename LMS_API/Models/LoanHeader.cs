namespace LMS_API.Models
{
    public class LoanHeader
    {
        public int LoanId { get; set; }

        public int BorrowerId { get; set; }
        public string? BorrowerName { get; set; } 

        public DateTime LoanDate { get; set; }
        public DateTime DueDate { get; set; }   
        public DateTime? ReturnDate { get; set; }
        public DateTime CreatedAt { get; set; }  

        public List<int> BookIds { get; set; } = new();
    }
}
