namespace SubtitlesTranslator.Application.DTOs {
    public class InvoicePdfResult {
        public string UserId { get; set; } = string.Empty;
        public string InvoiceNumber { get; set; } = string.Empty;
        public byte[] PdfBytes { get; set; } = Array.Empty<byte>();
    }
}
