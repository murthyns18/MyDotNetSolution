
namespace LMS_API.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public string? ClaimId { get; set; }
        public string? CountryRegion { get; set; }
        public string? ClaimStatus { get; set; }
        public string? Program { get; set; }
        public string? Activity { get; set; }
        public decimal Amount { get; set; }
        public string? FiscalYear { get; set; }
        public string? Quarter { get; set; }
        public string? ClaimIDOverride { get; set; }
        public string? Remarks { get; set; }
        public decimal? AmountOverride { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
