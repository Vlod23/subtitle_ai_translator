using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SubtitlesTranslator.Application.Interfaces;
using SubtitlesTranslator.Models;

namespace SubtitlesTranslator.Controllers {
    [Authorize]
    public class InvoicingController : BaseController
    {
        private readonly IInvoicingProfileService _invoicingProfile;

        public InvoicingController(IUserContextService userContext,
            IInvoicingProfileService invoicingProfile)
            : base(userContext)
        {
            _invoicingProfile = invoicingProfile;
        }

        [HttpGet]
        public async Task<IActionResult> InvoicingDetails()
        {
            var userId = await _userContext.GetCurrentUserIdAsync();
            var model = await _invoicingProfile.GetAsync(userId);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveInvoicingDetails(InvoicingProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _invoicingProfile.SaveAsync(model);
                TempData["SuccessMsg"] = "Invoicing profile saved!";
            }
            else
            {
                TempData["ErrorMsg"] = "Something went wrong...";
            }

            return View("InvoicingDetails", model);
        }
    }
}
