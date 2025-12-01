namespace SubtitlesTranslator.Application.Interfaces
{
    public interface ISubtitleRepository
    {
        Task SaveAsync(SubtitleTranslation subtitle);
        Task UpdateAsync(SubtitleTranslation subtitle);
        Task<SubtitleTranslation?> GetUserSubtitleAsync(int subtitleId);
        Task<IEnumerable<SubtitleTranslation>> GetPublicSubtitlesAsync();
        Task SaveChangesAsync();
        Task DeleteAsync(SubtitleTranslation subtitle);
    }
}
