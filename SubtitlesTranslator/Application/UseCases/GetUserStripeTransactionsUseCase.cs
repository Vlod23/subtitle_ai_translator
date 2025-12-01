using Stripe.Checkout;
using SubtitlesTranslator.Application.Interfaces;
using SubtitlesTranslator.Models;

namespace SubtitlesTranslator.Application.UseCases {
    public class GetUserStripeTransactionsUseCase 
    {
        //public async Task<List<TransactionHistoryViewModel>> ExecuteAsync(string stripeId) 
        //{
        //    if (string.IsNullOrEmpty(stripeId)) 
        //        return new List<TransactionHistoryViewModel>();

        //    var service = new SessionService();
        //    var sessions = await service.ListAsync(new SessionListOptions {
        //        Customer = stripeId,
        //        Limit = 1000
        //    });

        //    var transactions = sessions.Data.Select(s => new TransactionHistoryViewModel {
        //        Id = s.Id,
        //        Amount = s.AmountTotal,
        //        Currency = s.Currency,
        //        CreatedAt = s.Created,
        //        Status = s.PaymentStatus
        //    }).ToList();

        //    return transactions;
        //}        
    }
}
