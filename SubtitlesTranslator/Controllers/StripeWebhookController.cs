using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Checkout;
using SubtitlesTranslator.Application.Interfaces;
using SubtitlesTranslator.Application.Services;
using SubtitlesTranslator.Application.UseCases;
using SubtitlesTranslator.Data;
using SubtitlesTranslator.Infrastructure.Services;

namespace SubtitlesTranslator.Controllers 
{
    [Route("Webhook")]
    [ApiController]
    public class StripeWebhookController : BaseController
    {
        private readonly IConfiguration _config;
        private readonly UpdateUserCreditsUseCase _updateUserCredits;
        private readonly CreatePaymentTransactionUseCase _createPaymentTransaction;
        private readonly SendMailWithInvoiceUseCase _sendMailWithInvoice;

        public StripeWebhookController(IConfiguration config, 
            UpdateUserCreditsUseCase updateUserCredits,
            IUserContextService userContext,
            CreatePaymentTransactionUseCase createPaymentTransaction,
            SendMailWithInvoiceUseCase sendMailWithInvoice) 
            : base(userContext) 
        {
            _config = config;
            _updateUserCredits = updateUserCredits;
            _createPaymentTransaction = createPaymentTransaction;
            _sendMailWithInvoice = sendMailWithInvoice;
        }

        [HttpPost]
        public async Task<IActionResult> Index() {
            try {
                var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
                var secret = _config["Stripe:WebhookSecret"];
                var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], secret, throwOnApiVersionMismatch: false);

                if (stripeEvent.Type == EventTypes.CheckoutSessionCompleted) {
                    var session = stripeEvent.Data.Object as Session;
                    var userId = session.Metadata["UserId"];
                    var credits = int.Parse(session.Metadata["Credits"]);

                    await _updateUserCredits.ExecuteAsync(userId, credits, add: true);

                    var totalAmount = session.AmountTotal / 100; // Convert from cents
                    var transactionId = await _createPaymentTransaction.ExecuteAsync(userId, totalAmount, credits, "Stripe", session.Currency, session.CustomerId);

                    await _sendMailWithInvoice.ExecuteAsync(transactionId);
                }

                return Ok();

            } catch (Exception ex) {
                return BadRequest($"Webhook error: {ex.Message}");
            }
        }
    }

}
