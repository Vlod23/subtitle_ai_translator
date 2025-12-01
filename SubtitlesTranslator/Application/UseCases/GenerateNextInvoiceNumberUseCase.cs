using SubtitlesTranslator.Application.Interfaces;

namespace SubtitlesTranslator.Application.UseCases {
    public class GenerateNextInvoiceNumberUseCase 
    {
        private readonly IInvoiceCounterRepository _counterRepository;

        public GenerateNextInvoiceNumberUseCase(IInvoiceCounterRepository counterRepository) 
        {
            _counterRepository = counterRepository;
        }

        public async Task<string> ExecuteAsync() 
        {
            var year = DateTime.Now.Year;
            var sequence = await _counterRepository.GetNextInvoiceSequenceAsync(year);
            return $"{year}-{sequence:D6}";
        }
    }
}
