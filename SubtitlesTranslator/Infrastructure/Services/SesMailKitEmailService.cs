using System.Threading.Tasks;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using SubtitlesTranslator.Application.Interfaces;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace SubtitlesTranslator.Infrastructure.Services {
    public class SesMailKitEmailService : IEmailService, IEmailSender 
    {
        private readonly IConfiguration _config;

        public SesMailKitEmailService(IConfiguration config) 
        {
            _config = config;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string htmlContent) 
        {
            await SendEmailAsync(toEmail, toName: toEmail, subject, plainTextBody: "developer testing", htmlContent);
        }

        public async Task SendEmailAsync(string toEmail, string toName, string subject, string plainTextBody, string htmlBody, IEnumerable<EmailAttachment>? attachments = null) 
        {
            var fromEmail = _config["Email:FromEmail"];
            var fromName = _config["Email:FromName"];
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(fromName, fromEmail));
            message.To.Add(new MailboxAddress(toName, toEmail));
            message.Subject = subject;

            var builder = new BodyBuilder 
            {
                TextBody = plainTextBody, // vraj zvýši šancu doručenia
                HtmlBody = htmlBody
            };

            if (attachments != null) 
            {
                foreach (var a in attachments) {
                    builder.Attachments.Add(a.FileName, a.ByteContent, ContentType.Parse(a.ContentType));
                }
            }

            message.Body = builder.ToMessageBody();

            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            var host = _config["Email:Host"]!;
            var port = int.Parse(_config["Email:Port"]!);
            var user = _config["Email:Username"]!;
            var pass = _config["Email:Password"]!;

            await smtp.ConnectAsync(host, port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(user, pass);
            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);
        }
    }

    public class EmailAttachment
    {
        public string FileName { get; set; }
        public byte[] ByteContent { get; set; }
        public string ContentType { get; set; }
    }
}
