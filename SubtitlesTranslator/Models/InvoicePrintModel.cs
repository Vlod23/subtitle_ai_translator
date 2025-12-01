namespace SubtitlesTranslator.Models {
    public class InvoicePrintModel
    {
        // App owner
        public string AppName { get; set; }
        public string AppAddress { get; set; }
        public string AppCity { get; set; }
        public string AppZip { get; set; }
        public string AppTaxId { get; set; }
        public string AppContact { get; set; }

        // User
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserCompanyName { get; set; }
        public string UserAddress { get; set; }
        public string UserHouseNo { get; set; }
        public string UserCity { get; set; }
        public string UserZip { get; set; }
        public string UserCountry { get; set; }
        public string UserTaxId { get; set; }

        // Invoice
        public string InvoiceNumber { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentType { get; set; }
        public decimal PaymentAmount { get; set; }
        public string PaymentCurrency { get; set; }
        public int Credits { get; set; }
    }
}
