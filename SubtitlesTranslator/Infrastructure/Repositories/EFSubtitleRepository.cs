using Microsoft.EntityFrameworkCore;
using SubtitlesTranslator.Application.Interfaces;
using SubtitlesTranslator.Data;

namespace SubtitlesTranslator.Infrastructure.Repositories
{
    public class EFSubtitleRepository : ISubtitleRepository
    {
        private readonly ApplicationDbContext _context;

        public EFSubtitleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SubtitleTranslation?> GetUserSubtitleAsync(int subtitleId)
        {
            return await _context.SubtitleTranslations
                .FirstOrDefaultAsync(s => s.Id == subtitleId);
        }

        public async Task<IEnumerable<SubtitleTranslation>> GetPublicSubtitlesAsync()
        {
            return await _context.SubtitleTranslations
                .Where(s => s.IsPublic)
                .OrderBy(s => s.CreatedAt)
                .Include(s => s.User)
                .Include(s => s.Likes)
                .ToListAsync();
        }

        public async Task SaveAsync(SubtitleTranslation subtitle)
        {
            _context.SubtitleTranslations.Add(subtitle);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(SubtitleTranslation subtitle) {
            _context.SubtitleTranslations.Update(subtitle);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(SubtitleTranslation subtitle)
        {
            _context.SubtitleTranslations.Remove(subtitle);
            await SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }

}
