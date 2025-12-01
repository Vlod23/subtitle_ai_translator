using Stripe;
using Stripe.Checkout;

namespace SubtitlesTranslator.Application.Interfaces {
    public interface IStripeClientAdapter
    {
        Task<Customer> CreateCustomerAsync(string email);
        Task<Session> CreateSessionAsync(SessionCreateOptions options);
    }
}
