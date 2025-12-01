using Stripe;
using Stripe.Checkout;
using SubtitlesTranslator.Application.Interfaces;

namespace SubtitlesTranslator.Application.UseCases {
    public class CreateCheckoutSessionUseCase 
    {
        private readonly IUserRepository _userRepository;
        private readonly SaveStripeCustomerIdUseCase _saveStripeCustomerId;
        private readonly IConfiguration _config;
        private readonly IStripeClientAdapter _stripeClientAdapter;

        public CreateCheckoutSessionUseCase(
            IUserRepository userRepository,
            SaveStripeCustomerIdUseCase saveCustomerIdUseCase,
            IConfiguration config,
            IStripeClientAdapter stripeClientAdapter) 
        {
            _userRepository = userRepository;
            _saveStripeCustomerId = saveCustomerIdUseCase;
            _config = config;
            _stripeClientAdapter = stripeClientAdapter;
        }

        public async Task<string> ExecuteAsync(string userId, int creditAmount, string domainUrl) 
        {
            var user = await _userRepository.GetUserByIdAsync(userId);

            // Create or reuse Stripe Customer
            var customerId = user.StripeCustomerId;
            if (string.IsNullOrEmpty(customerId)) {
                var stripeCustomer = await _stripeClientAdapter.CreateCustomerAsync(user.Email);
                customerId = stripeCustomer.Id;

                await _saveStripeCustomerId.ExecuteAsync(userId, customerId);
            }

            var options = new SessionCreateOptions {
                PaymentMethodTypes = new List<string> { "card" },
                Customer = customerId,
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = GetPriceInCents(creditAmount),
                            Currency = "eur",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = $"{creditAmount} subtitle translation credits"
                            }
                        },
                        Quantity = 1
                    }
                },
                Mode = "payment",
                SuccessUrl = $"{domainUrl}/Credits/Success",
                CancelUrl = $"{domainUrl}/Credits/Cancel",
                Metadata = new Dictionary<string, string>
                {
                    { "UserId", user.Id },
                    { "Credits", creditAmount.ToString() }
                }
            };

            var session = await _stripeClientAdapter.CreateSessionAsync(options);

            return session.Url;
        }

        private long GetPriceInCents(int credits) {
            int creditsPerEuro = int.Parse(_config["CreditSystem:CreditsPerEuro"]);
            return credits * 100 / creditsPerEuro; // Converts to cents
        }
    }

}
