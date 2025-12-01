using SubtitlesTranslator.Domain.Entities;
using SubtitlesTranslator.Models;

namespace SubtitlesTranslator.Application.Interfaces {
    public interface IInvoicingProfileService
    {
        Task<InvoicingProfileViewModel?> GetAsync(string userId);
        Task SaveAsync(InvoicingProfileViewModel viewModel);
    }
}
