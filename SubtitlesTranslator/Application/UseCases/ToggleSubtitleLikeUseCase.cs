using SubtitlesTranslator.Application.Interfaces;
using SubtitlesTranslator.Domain.Entities;

namespace SubtitlesTranslator.Application.UseCases {
    public class ToggleSubtitleLikeUseCase {
        private readonly ISubtitleLikeRepository _repository;

        public ToggleSubtitleLikeUseCase(ISubtitleLikeRepository repository) {
            _repository = repository;
        }

        public async Task<int> ExecuteAsync(string userId, int subtitleId) {
            var existingLike = await _repository.GetLikeAsync(userId, subtitleId);

            if (existingLike != null) {
                await _repository.RemoveLikeAsync(existingLike);
            } else {
                var newLike = new SubtitleLike {
                    UserId = userId,
                    SubtitleTranslationId = subtitleId
                };
                await _repository.AddLikeAsync(newLike);
            }

            return await _repository.GetLikesCountAsync(subtitleId);
        }
    }
}
