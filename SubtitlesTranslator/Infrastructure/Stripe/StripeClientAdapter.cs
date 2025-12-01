using Stripe;
using Stripe.Checkout;
using SubtitlesTranslator.Application.Interfaces;

namespace SubtitlesTranslator.Infrastructure.Stripe {
    public class StripeClientAdapter : IStripeClientAdapter {
        private readonly CustomerService _customers;
        private readonly SessionService _session;

        public StripeClientAdapter(
          CustomerService customers,
          SessionService session) 
        {
            _customers = customers;
            _session = session;
        }

        public Task<Customer> CreateCustomerAsync(string email) =>
          _customers.CreateAsync(new CustomerCreateOptions { Email = email });

        public Task<Session> CreateSessionAsync(SessionCreateOptions opts) =>
          _session.CreateAsync(opts);
    }

}
