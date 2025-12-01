namespace SubtitlesTranslator.Domain.Entities {
    public class InvoicingProfile
    {
        public string UserId { get; set; }   // FK back to ApplicationUser
        public string CompanyName { get; set; }
        public string TaxIdentifier { get; set; }
        public string AddressLine1 { get; set; }
        public string HouseNumber { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }

        // Navigation (foreign key)
        public ApplicationUser User { get; set; }
    }
}
