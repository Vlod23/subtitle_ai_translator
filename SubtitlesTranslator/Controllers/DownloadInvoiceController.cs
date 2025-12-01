using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SubtitlesTranslator.Application.UseCases;

namespace SubtitlesTranslator.Controllers {
    public class DownloadInvoiceController : Controller 
    {
        private readonly GeneratePDFInvoiceUseCase _generatePDFInvoice;

        public DownloadInvoiceController(GeneratePDFInvoiceUseCase generatePDFInvoice) 
        {
            _generatePDFInvoice = generatePDFInvoice;
        }

        [HttpGet]
        public async Task<IActionResult> Download(int transactionId) 
        {
            var result = await _generatePDFInvoice.ExecuteAsync(transactionId);

            if (result.PdfBytes == null || result.PdfBytes.Length == 0 || string.IsNullOrEmpty(result.InvoiceNumber))
                return NotFound();

            var fileName = $"Invoice-{result.InvoiceNumber}.pdf";

            return File(result.PdfBytes, "application/pdf", fileName);
        }

    }
}
