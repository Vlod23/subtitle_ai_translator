namespace SubtitlesTranslator.Models {
    public class InvoicingProfileViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string TaxIdentifier { get; set; } = string.Empty;
        public string AddressLine1 { get; set; } = string.Empty;
        public string HouseNumber { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;

    }
}
