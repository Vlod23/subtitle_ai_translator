namespace SubtitlesTranslator.Models {
    public class TransactionHistoryViewModel
    {
        public int Id { get; set; }
        public decimal Amount { get; set; } // In cents
        public string Currency { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Credits { get; set; }
        public string InvoiceNumber { get; set; }
    }
}
