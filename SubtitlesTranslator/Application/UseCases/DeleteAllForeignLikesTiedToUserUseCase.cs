using SubtitlesTranslator.Application.Interfaces;

namespace SubtitlesTranslator.Application.UseCases {
    public class DeleteAllForeignLikesTiedToUserUseCase {
        private readonly ISubtitleLikeRepository _subtitleLikeRepository;
        private readonly IUserSubtitleService _userSubtitleService;

        public DeleteAllForeignLikesTiedToUserUseCase(
            ISubtitleLikeRepository subtitleLikeRepository,
            IUserSubtitleService userSubtitleService) {
            _subtitleLikeRepository = subtitleLikeRepository;
            _userSubtitleService = userSubtitleService;
        }

        public async Task<bool> ExecuteAsync(string userId)
        {
            try {
                var subtitles = await _userSubtitleService.GetUserSubtitlesAsync(userId);

                foreach (var subtitle in subtitles) {
                    await _subtitleLikeRepository.DeleteAllLikesTiedToSubtitleAsync(subtitle.Id);
                }
                return true;

            } catch {
                // TODO: Log Error
                return false;
            }

        }
    }
}
