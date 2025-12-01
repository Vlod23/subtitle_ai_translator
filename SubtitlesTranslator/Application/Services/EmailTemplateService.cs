using SubtitlesTranslator.Application.Interfaces;

namespace SubtitlesTranslator.Application.Services {
    public class EmailTemplateService : IEmailTemplateService 
    {
        private readonly IWebHostEnvironment _env;
        private readonly string? _baseUrl;

        public EmailTemplateService(IConfiguration config, IWebHostEnvironment env) {
            _baseUrl = config["App:BaseUrl"];
            _env = env;
        }

        public string GetMailSubjectConfirmEmail() {
            return "Confirm your email";
        }

        public string GetMailBodyConfirmEmail(string callbackUrl) {
            var templatePath = Path.Combine(_env.ContentRootPath, "Templates", "Emails", "ConfirmEmailTemplate.html");
            var html = File.ReadAllText(templatePath);

            return html.Replace("{{callbackUrl}}", callbackUrl);
        }

        public string GetDownloadUrl(int subtitleId) {
            return $"{_baseUrl}/Download/DownloadTranslated/{subtitleId}";
        }
        public string GetMyTranslationsUrl() {
            return $"{_baseUrl}/SubtitleManagement/MyTranslations";
        }

        public string GetMailSubjectTranslated() {
            return "Your subtitle has been translated!";
        }
        public string GetMailBodyTranslated(int subtitleId, string subtitleName) {
            var downloadUrl = GetDownloadUrl(subtitleId);
            var myTranslationsUrl = GetMyTranslationsUrl();

            return $@"<p>Your subtitle <b>{subtitleName}</b> was translated successfully! 🎉</p>
                    <p><a href='{downloadUrl}'>⬇ Click here to download</a></p>
                    <p>You can also view all your translations <a href='{myTranslationsUrl}'>HERE</a>.</p>";
        }

        public string GetMailSubjectTranslationFailed() {
            return "Your subtitle translation failed.";
        }
        public string GetMailBodyTranslationFailed(int subtitleId, string subtitleName) {
            var myTranslationsUrl = GetMyTranslationsUrl();

            return $@"<p>Your subtitle <b>{subtitleName}</b> translation failed. 😢</p>
                    <p>Credits have been returned to your account.</p>
                    <p>You can also view all your translations <a href='{myTranslationsUrl}'>HERE</a>.</p>";
        }

        public string GetMailSubjectPaymentSuccess() {
            return "Payment successful!";
        }
        public string GetMailBodyPaymentSuccess(int credits) {
            var myTranslationsUrl = GetMyTranslationsUrl();

            return $@"<p>Your payment was successful! 🎉</p>
                    <p>You have received <b>{credits}</b> credits to your account.</p>
                    <p>Find an invoice attached to this email.</p>
                    <br />
                    <p>Visit your dashboard <a href='{myTranslationsUrl}'>HERE</a>.</p>";
        }
    }
}
