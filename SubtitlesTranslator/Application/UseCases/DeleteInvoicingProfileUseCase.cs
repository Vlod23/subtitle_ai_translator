using SubtitlesTranslator.Application.Interfaces;

namespace SubtitlesTranslator.Application.UseCases {
    public class DeleteInvoicingProfileUseCase
    {
        private readonly IInvoicingProfileRepository _invoicingProfileRepository;

        public DeleteInvoicingProfileUseCase(IInvoicingProfileRepository invoicingProfileRepository) {
            _invoicingProfileRepository = invoicingProfileRepository;
        }
        
        public async Task<bool> ExecuteAsync(string userId) {
            try {
                var invProfile = await _invoicingProfileRepository.GetByUserIdAsync(userId);
                if (invProfile == null) return false;

                await _invoicingProfileRepository.DeleteInvoicingProfile(invProfile);

                return true;

            } catch {
                File.AppendAllText("log.txt", "An error occurred while deleting the invoicing profile");
                File.AppendAllText("log.txt", Environment.NewLine);
                return false;
            }

        }
    }
}
