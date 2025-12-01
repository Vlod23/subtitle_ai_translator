using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SubtitlesTranslator.Application.Helpers;
using SubtitlesTranslator.Application.Interfaces;
using SubtitlesTranslator.Models;

namespace SubtitlesTranslator.Application.Services {
    public class QuestPDFGenerator : IQuestPDFGenerator
    {
        public byte[] Generate(InvoicePrintModel m) {
            return Document.Create(doc => {
                doc.Page(page => {
                    page.Size(PageSizes.A4);
                    page.Margin(40);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header().PaddingTop(20).Row(row => {
                        row.RelativeItem().Column(col => {
                            col.Item().Text(m.AppName).Bold().FontSize(18);
                            col.Item().PaddingTop(10).Text($"{m.AppAddress}");
                            col.Item().Text($"{m.AppZip} {m.AppCity}");
                            col.Item().Text($"Tax ID: {m.AppTaxId}");
                            col.Item().Text($"Contact: {m.AppContact}");
                        });
                        row.RelativeItem().Column(col => {
                            col.Item().PaddingTop(5).Text($"Invoice #{m.InvoiceNumber}").Bold();
                            col.Item().PaddingTop(10).Text($"Date: {m.PaymentDate:yyyy-MM-dd}");
                            col.Item().Text($"Method: {m.PaymentType}");
                        });
                    });

                    page.Content().Column(column =>
                    {
                        column.Item().PaddingVertical(10).LineHorizontal(1).LineColor(Colors.Grey.Medium);
                        column.Item().PaddingVertical(10).Column(bill =>
                        {
                            bill.Item().Text("Bill To:").SemiBold();
                            bill.Item().PaddingTop(10).Text(string.IsNullOrEmpty(m.UserCompanyName) ? m.UserName : m.UserCompanyName);
                            bill.Item().Text($"{m.UserAddress} {m.UserHouseNo}");
                            bill.Item().Text($"{m.UserZip} {m.UserCity}, {m.UserCountry}");
                            if (!string.IsNullOrEmpty(m.UserTaxId)) 
                            {
                                bill.Item().Text($"Tax ID: {m.UserTaxId}");
                            }                                
                            bill.Item().Text($"Email: {m.UserEmail}");
                        });

                        column.Item().PaddingVertical(10).LineHorizontal(1).LineColor(Colors.Grey.Medium);

                        column.Item().PaddingVertical(10).Row(row =>
                        {
                            row.RelativeItem().Column(left =>
                            {
                                left.Item().Text("Credits Purchased").FontSize(15).Bold();
                                left.Item().PaddingTop(10).Text($"{m.Credits}").FontSize(20).FontColor(Colors.Blue.Medium);
                            });
                            row.RelativeItem().Column(right =>
                            {
                                var currencySymbol = CurrencySymbolProvider.GetSymbol(m.PaymentCurrency);

                                right.Item().Text("Amount Paid").FontSize(15).Bold();
                                right.Item().PaddingTop(10).Text($"{m.PaymentAmount:N2} {currencySymbol}").FontSize(20).FontColor(Colors.Green.Medium);
                            });
                        });
                    });

                    page.Footer()
                    .AlignCenter()
                    .Text("Thank you for your purchase!")
                    .FontSize(10);
                });
            })
            .GeneratePdf();
        }
    }
}
