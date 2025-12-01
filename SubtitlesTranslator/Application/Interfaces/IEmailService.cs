using SubtitlesTranslator.Infrastructure.Services;

namespace SubtitlesTranslator.Application.Interfaces {
    public interface IEmailService 
    {
        Task SendEmailAsync(string toEmail, string toName, string subject, string plainTextBody, string htmlBody, IEnumerable<EmailAttachment>? attachments = null);
    }
}
