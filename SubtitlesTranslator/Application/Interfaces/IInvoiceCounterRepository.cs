namespace SubtitlesTranslator.Application.Interfaces {
    public interface IInvoiceCounterRepository
    {
        Task<int> GetNextInvoiceSequenceAsync(int year);
    }
}
