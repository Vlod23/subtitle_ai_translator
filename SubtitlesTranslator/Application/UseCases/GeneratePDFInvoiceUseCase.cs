using SubtitlesTranslator.Application.DTOs;
using SubtitlesTranslator.Application.Interfaces;
using SubtitlesTranslator.Models;

namespace SubtitlesTranslator.Application.UseCases 
{
    public class GeneratePDFInvoiceUseCase 
    {
        private readonly IPaymentTransactionRepository _paymentTransactionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IInvoicingProfileRepository _invoicingProfileRepository;
        private readonly IConfiguration _config;
        private readonly IQuestPDFGenerator _PDFGenerator;

        public GeneratePDFInvoiceUseCase(
            IPaymentTransactionRepository paymentTransactionRepository,
            IUserRepository userRepository,
            IInvoicingProfileRepository invoicingProfileRepository,
            IConfiguration config,
            IQuestPDFGenerator PDFGenerator) 
        {
            _paymentTransactionRepository = paymentTransactionRepository;
            _userRepository = userRepository;
            _invoicingProfileRepository = invoicingProfileRepository;
            _config = config;
            _PDFGenerator = PDFGenerator;
        }

        public async Task<InvoicePdfResult> ExecuteAsync(object id, bool isIntId) 
        {
            var transaction = new Domain.Entities.PaymentTransaction();

            if (isIntId) {
                if (int.TryParse(id.ToString(), out var parsedId)) 
                {
                    transaction = await _paymentTransactionRepository.GetByIdAsync(parsedId);
                }
            } else {
                var invoiceNo = id?.ToString() ?? string.Empty;
                transaction = await _paymentTransactionRepository.GetByInvoiceNumberAsync(invoiceNo);
            }
            if (transaction == null)
                return new InvoicePdfResult();
            
            var user = await _userRepository.GetUserByIdAsync(transaction.UserId);
            var userInvoiceProfile = await _invoicingProfileRepository.GetByUserIdAsync(transaction.UserId);

            var model = new InvoicePrintModel();

            model.AppName = _config["AppDetails:AppName"] ?? "";
            model.AppAddress = _config["AppDetails:AppAddress"] ?? "";
            model.AppCity = _config["AppDetails:AppCity"] ?? "";
            model.AppZip = _config["AppDetails:AppZip"] ?? "";
            model.AppTaxId = _config["AppDetails:AppTaxId"] ?? "";
            model.AppContact = _config["AppDetails:AppEmail"] ?? "";

            model.UserName = user.UserName;
            model.UserEmail = user.Email;
            model.UserCompanyName = userInvoiceProfile.CompanyName;
            model.UserAddress = userInvoiceProfile.AddressLine1;
            model.UserHouseNo = userInvoiceProfile.HouseNumber;
            model.UserCity = userInvoiceProfile.City;
            model.UserZip = userInvoiceProfile.PostalCode;
            model.UserTaxId = userInvoiceProfile.TaxIdentifier;

            model.PaymentDate = transaction.TransactionDate;
            model.PaymentAmount = transaction.Amount;
            model.Credits = transaction.CreditsPurchased;
            model.PaymentType = transaction.PaymentMethod;
            model.PaymentCurrency = transaction.Currency;
            model.InvoiceNumber = transaction.InvoiceNumber;

            var result = new InvoicePdfResult
            {
                UserId = transaction.UserId,
                PdfBytes = _PDFGenerator.Generate(model),
                InvoiceNumber = transaction.InvoiceNumber
            };

            return result;
        }
    }
}
