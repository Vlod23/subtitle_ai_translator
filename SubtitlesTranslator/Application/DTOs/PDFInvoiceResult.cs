namespace SubtitlesTranslator.Application.DTOs {
    public class InvoicePdfResult {
        public string InvoiceNumber { get; set; }
        public byte[] PdfBytes { get; set; }
    }
}
