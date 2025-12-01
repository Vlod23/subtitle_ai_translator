using SubtitlesTranslator.Domain.Entities;

namespace SubtitlesTranslator.Application.Interfaces {
    public interface IInvoicingProfileRepository
    {
        Task<InvoicingProfile?> GetByUserIdAsync(string userId);
        Task SaveAsync(InvoicingProfile profile);
        Task DeleteInvoicingProfile(InvoicingProfile invoicingProfile);
    }
}
