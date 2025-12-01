using Microsoft.EntityFrameworkCore;
using SubtitlesTranslator.Application.Interfaces;
using SubtitlesTranslator.Data;

namespace SubtitlesTranslator.Infrastructure.Repositories
{
    public class EFUserSubtitleService : IUserSubtitleService
    {
        private readonly ApplicationDbContext _context;

        public EFUserSubtitleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<SubtitleTranslation>> GetUserSubtitlesAsync(string userId)
        {
            return await _context.SubtitleTranslations
                .Where(s => s.UserId == userId)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
        }

        public async Task<TranslationDetailViewModel?> GetUserSubtitleDetailsAsync(int subtitleId, string userId)
        {
            var subtitle = await _context.SubtitleTranslations
                //.Include(s => s.User)
                .FirstOrDefaultAsync(s => s.Id == subtitleId && s.UserId == userId);

            if (subtitle == null)
                return null;

            var translationDetails = new TranslationDetailViewModel {
                Id = subtitle.Id,
                Name = subtitle.Name,
                Year = subtitle.Year,
                Director = subtitle.Director,
                OriginalText = subtitle.OriginalText,
                TranslatedText = subtitle.TranslatedText,
                TargetLanguage = subtitle.TargetLanguage,
                CreatedAt = subtitle.CreatedAt,
                IsPublic = subtitle.IsPublic,
                CreditsUsed = subtitle.CreditsUsed
            };

            return translationDetails;
        }
    }
}
