namespace SubtitlesTranslator.Application.Interfaces
{
    public interface IChatGptService
    {
        Task<string> TranslateAsync(string originalText, string prompt, string targetLanguage);
        public string SetPrompt(string targetLanguage);
    }
}
