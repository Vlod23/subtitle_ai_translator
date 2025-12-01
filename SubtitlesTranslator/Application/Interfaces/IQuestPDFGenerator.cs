using SubtitlesTranslator.Models;

namespace SubtitlesTranslator.Application.Interfaces {
    public interface IQuestPDFGenerator 
    {
        byte[] Generate(InvoicePrintModel model);
    }
}
