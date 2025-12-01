using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Checkout;
using SubtitlesTranslator.Application.Interfaces;
using SubtitlesTranslator.Application.UseCases;
using SubtitlesTranslator.Data;


namespace SubtitlesTranslator.Controllers {
    public class CreditsController : BaseController {
        private readonly CreateCheckoutSessionUseCase _createCheckoutSession;

        public CreditsController(
        IUserContextService userContext,
        CreateCheckoutSessionUseCase createCheckoutSession) 
            : base(userContext) 
        {
            _createCheckoutSession = createCheckoutSession;
        }

        [HttpGet]
        public IActionResult BuyCredits() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCheckoutSession(int creditAmount) {
            var domain = $"{Request.Scheme}://{Request.Host}";
            var userId = await _userContext.GetCurrentUserIdAsync();
            var sessionUrl = await _createCheckoutSession.ExecuteAsync(userId, creditAmount, domain);

            // Vytvoriť Transaction record a uložiť ho do db novej tabuľky
            return Redirect(sessionUrl);
        }

        public IActionResult Success() {
            TempData["SuccessMsg"] = "Payment successful! Your credits will be updated shortly.";
            return RedirectToAction("MyTranslations", "SubtitleManagement");

        }

        public IActionResult Cancel() {
            TempData["ErrorMsg"] = "Payment cancelled.";
            return RedirectToAction("MyTranslations", "SubtitleManagement");

        }
    }
}
