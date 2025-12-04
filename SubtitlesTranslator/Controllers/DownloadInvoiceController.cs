using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SubtitlesTranslator.Application.Authorization;
using SubtitlesTranslator.Application.Interfaces;
using SubtitlesTranslator.Application.UseCases;

namespace SubtitlesTranslator.Controllers {
    [Authorize]
    public class DownloadInvoiceController : BaseController
    {
        private readonly GeneratePDFInvoiceUseCase _generatePDFInvoice;
        private readonly IUserContextService _userContext;

        public DownloadInvoiceController(GeneratePDFInvoiceUseCase generatePDFInvoice, IUserContextService userContext) : base(userContext)
        {
            _generatePDFInvoice = generatePDFInvoice;
            _userContext = userContext;
        }

        [HttpGet]
        public async Task<IActionResult> Download(int transactionId)
        {
            var result = await _generatePDFInvoice.ExecuteAsync(transactionId, isIntId: true);

            if (result.PdfBytes == null || result.PdfBytes.Length == 0 || string.IsNullOrEmpty(result.InvoiceNumber))
                return NotFound();

            var currentUserId = await _userContext.GetCurrentUserIdAsync();
            var hasInvoicePermission = User.HasClaim(AuthorizationConstants.PermissionClaimType, PermissionNames.GenerateInvoices);

            if (!hasInvoicePermission && !string.Equals(result.UserId, currentUserId, StringComparison.Ordinal))
                return Forbid();

            var fileName = $"Invoice-{result.InvoiceNumber}.pdf";

            return File(result.PdfBytes, "application/pdf", fileName);
        }

    }
}
