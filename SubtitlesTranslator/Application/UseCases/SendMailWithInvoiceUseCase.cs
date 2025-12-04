using SubtitlesTranslator.Application.Interfaces;
using SubtitlesTranslator.Infrastructure.Services;

namespace SubtitlesTranslator.Application.UseCases {
    public class SendMailWithInvoiceUseCase 
    {
        private readonly IEmailService _emailService;
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly IUserRepository _userRepository;
        private readonly IPaymentTransactionRepository _paymentTransactionRepository;
        private readonly GeneratePDFInvoiceUseCase _generatePDFInvoice;

        public SendMailWithInvoiceUseCase(
            IEmailService emailService,
            IEmailTemplateService emailTemplateService,
            IUserRepository userRepository,
            IPaymentTransactionRepository paymentTransactionRepository,
            GeneratePDFInvoiceUseCase generatePDFInvoice) 
        {
            _emailService = emailService;
            _emailTemplateService = emailTemplateService;
            _userRepository = userRepository;
            _paymentTransactionRepository = paymentTransactionRepository;
            _generatePDFInvoice = generatePDFInvoice;
        }

        public async Task ExecuteAsync(int transactionId) 
        {
            var transaction = await _paymentTransactionRepository.GetByIdAsync(transactionId);
            var user = await _userRepository.GetUserByIdAsync(transaction.UserId);
            var mailSubject = _emailTemplateService.GetMailSubjectPaymentSuccess();
            var htmlBodyContent = _emailTemplateService.GetMailBodyPaymentSuccess(transaction.CreditsPurchased);

            var pdfResult = await _generatePDFInvoice.ExecuteAsync(transactionId, isIntId: true);
            if (pdfResult.PdfBytes == null || pdfResult.PdfBytes.Length == 0 || string.IsNullOrEmpty(pdfResult.InvoiceNumber)) {
                throw new Exception("Failed to generate PDF invoice.");
            }
            var attachment = new EmailAttachment {
                FileName = $"Invoice-{transaction.InvoiceNumber}.pdf",
                ByteContent = pdfResult.PdfBytes,
                ContentType = "application/pdf"
            };
            IEnumerable<EmailAttachment> attachments = new List<EmailAttachment> { attachment };

            await _emailService.SendEmailAsync(user.Email, user.UserName, mailSubject, "developer testing", htmlBodyContent, attachments);
        }
    }
}
