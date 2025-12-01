using SubtitlesTranslator.Domain.Entities;

namespace SubtitlesTranslator.Application.Interfaces {
    public interface ISubtitleLikeRepository {
        Task<SubtitleLike?> GetLikeAsync(string userId, int subtitleId);
        Task AddLikeAsync(SubtitleLike like);
        Task RemoveLikeAsync(SubtitleLike like);
        Task<int> GetLikesCountAsync(int subtitleId);
        Task DeleteAllLikesTiedToSubtitleAsync(int subtitleId);
    }
}
