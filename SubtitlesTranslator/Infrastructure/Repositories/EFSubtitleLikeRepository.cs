using Microsoft.EntityFrameworkCore;
using SubtitlesTranslator.Application.Interfaces;
using SubtitlesTranslator.Data;
using SubtitlesTranslator.Domain.Entities;

namespace SubtitlesTranslator.Infrastructure.Repositories {
    public class EFSubtitleLikeRepository : ISubtitleLikeRepository {
        private readonly ApplicationDbContext _context;

        public EFSubtitleLikeRepository(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<SubtitleLike?> GetLikeAsync(string userId, int subtitleId) {
            return await _context.SubtitleLikes
                .FirstOrDefaultAsync(x => x.UserId == userId && x.SubtitleTranslationId == subtitleId);
        }

        public async Task AddLikeAsync(SubtitleLike like) {
            _context.SubtitleLikes.Add(like);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveLikeAsync(SubtitleLike like) {
            _context.SubtitleLikes.Remove(like);
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetLikesCountAsync(int subtitleId) {
            return await _context.SubtitleLikes
                .CountAsync(x => x.SubtitleTranslationId == subtitleId);
        }

        public async Task DeleteAllLikesTiedToSubtitleAsync(int subtitleId) {
            var likes = await _context.SubtitleLikes
                .Where(x => x.SubtitleTranslationId == subtitleId)
                .ToListAsync();

            if (likes != null && likes.Count > 0) {
                _context.SubtitleLikes.RemoveRange(likes);
                await _context.SaveChangesAsync();
            }
        }
    }
}
