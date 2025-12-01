using SubtitlesTranslator.Application.Interfaces;
using SubtitlesTranslator.Domain.Entities;

namespace SubtitlesTranslator.Application.UseCases {
    public class CreatePaymentTransactionUseCase 
    {
        private readonly IPaymentTransactionRepository _paymentTransactionRepository;
        private readonly GenerateNextInvoiceNumberUseCase _generateInvoiceNumber;

        public CreatePaymentTransactionUseCase(IPaymentTransactionRepository paymentTransactionRepository, GenerateNextInvoiceNumberUseCase generateInvoiceNumber) 
        {
            _paymentTransactionRepository = paymentTransactionRepository;
            _generateInvoiceNumber = generateInvoiceNumber;
        }

        public async Task<int> ExecuteAsync(string userId, long? amount, int credits, string paymentMethod, string currency, string stripeCustomerId) 
        {
            var transaction = new PaymentTransaction 
            {
                UserId = userId,
                Amount = (decimal)amount,
                Currency = currency,
                TransactionDate = DateTime.UtcNow,
                StripeChargeId = stripeCustomerId,
                CreditsPurchased = credits,
                InvoiceNumber = await _generateInvoiceNumber.ExecuteAsync(), 
                PaymentMethod = paymentMethod
            };

            await _paymentTransactionRepository.AddTransactionAsync(transaction);

            return transaction.Id;
        }
    }
}
