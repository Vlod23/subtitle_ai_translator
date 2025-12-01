using SubtitlesTranslator.Domain.Entities;

namespace SubtitlesTranslator.Application.Interfaces {
    public interface IPaymentTransactionRepository 
    {
        Task<PaymentTransaction> AddTransactionAsync(PaymentTransaction transaction);
        Task<List<PaymentTransaction>> GetByUserIdAsync(string userId);
        Task<PaymentTransaction?> GetByIdAsync(int id);
        Task<PaymentTransaction?> GetByInvoiceNumberAsync(string invoiceNumber);
    }
}
