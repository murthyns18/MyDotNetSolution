using LMS_API.Models;

namespace LMS_API.Interfaces
{
    public interface IPaymentRepository
    {
        IEnumerable<Payment> GetList(string? fiscalYear = null, string? quarter = null);
        string SavePayment(Payment payment);
        string DeletePayment(int paymentId);
        string BulkSavePayments(List<Payment> payments);
        string BulkUpdateOverrides(List<Payment> payments);
    }
}
