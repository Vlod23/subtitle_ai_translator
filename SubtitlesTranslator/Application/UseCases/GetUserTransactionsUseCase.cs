using System.Linq;
using Microsoft.AspNetCore.Http.HttpResults;
using SubtitlesTranslator.Application.Interfaces;
using SubtitlesTranslator.Models;

namespace SubtitlesTranslator.Application.UseCases {
    public class GetUserTransactionsUseCase 
    {
        private readonly IPaymentTransactionRepository _paymentTransactionRepository;
        public GetUserTransactionsUseCase(IPaymentTransactionRepository paymentTransactionRepository) 
        {
            _paymentTransactionRepository = paymentTransactionRepository;
        }

        public async Task<List<TransactionHistoryViewModel>> ExecuteAsync(string userId) 
        {
            var paymentTransactions = await _paymentTransactionRepository.GetByUserIdAsync(userId);

            var transactions = paymentTransactions.Select(p => new TransactionHistoryViewModel 
            {
                Id = p.Id,
                Amount = p.Amount,
                Currency = p.Currency,
                CreatedAt = p.TransactionDate,
                Credits = p.CreditsPurchased,
                InvoiceNumber = p.InvoiceNumber,
            })
            .OrderByDescending(t => t.CreatedAt)
            .ToList();

            return transactions;
        }
    }
}
