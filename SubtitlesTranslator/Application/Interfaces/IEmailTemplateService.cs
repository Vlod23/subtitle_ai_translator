namespace SubtitlesTranslator.Application.Interfaces {
    public interface IEmailTemplateService {
        public string GetMailSubjectConfirmEmail();
        public string GetMailBodyConfirmEmail(string callbackUrl);
        public string GetDownloadUrl(int subtitleId);
        public string GetMailSubjectTranslated();
        public string GetMailBodyTranslated(int subtitleId, string subtitleName);
        public string GetMailSubjectTranslationFailed();
        public string GetMailBodyTranslationFailed(int subtitleId, string subtitleName);
        public string GetMailSubjectPaymentSuccess();
        public string GetMailBodyPaymentSuccess(int credits);
    }
}
