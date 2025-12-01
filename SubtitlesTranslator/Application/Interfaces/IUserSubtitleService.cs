namespace SubtitlesTranslator.Application.Interfaces
{
    public interface IUserSubtitleService
    {
        Task<List<SubtitleTranslation>> GetUserSubtitlesAsync(string userId);
        Task<TranslationDetailViewModel?> GetUserSubtitleDetailsAsync(int subtitleId, string userId);
    }
}
