using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SubtitlesTranslator.Application.Authorization;
using SubtitlesTranslator.Application.UseCases;

namespace SubtitlesTranslator.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = PermissionNames.GenerateInvoices)]
    public class InvoicesController : Controller
    {
        private readonly GeneratePDFInvoiceUseCase _generatePdfInvoice;

        public InvoicesController(GeneratePDFInvoiceUseCase generatePdfInvoice)
        {
            _generatePdfInvoice = generatePdfInvoice;
        }

        [HttpGet]
        public async Task<IActionResult> Download(string invoiceNo)
        {
            var result = await _generatePdfInvoice.ExecuteAsync(invoiceNo, isIntId: false);

            if (result.PdfBytes == null || result.PdfBytes.Length == 0 || string.IsNullOrEmpty(result.InvoiceNumber)) {
                TempData["ErrorMsg"] = "This invoice number doesn't exist.";
                return RedirectToAction("Index", "Home");
            }

            var fileName = $"Invoice-{result.InvoiceNumber}.pdf";
            return File(result.PdfBytes, "application/pdf", fileName);
        }
    }
}
