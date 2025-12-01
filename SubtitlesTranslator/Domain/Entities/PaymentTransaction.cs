namespace SubtitlesTranslator.Domain.Entities {
    public class PaymentTransaction 
    {
        // Surrogate PK
        public int Id { get; set; }
        // FK back to AspNetUsers.Id 
        public string UserId { get; set; }
        // Navigation back to the user (optional)
        public DateTime TransactionDate { get; set; }
        public string StripeChargeId { get; set; }
        public int CreditsPurchased { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string InvoiceNumber { get; set; }
        public string? PaymentMethod { get; set; }
        public ApplicationUser User { get; set; } // Navigation property to the user entity
    }
}
