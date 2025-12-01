using SubtitlesTranslator.Application.Interfaces;

namespace SubtitlesTranslator.Application.UseCases
{
    public class DeleteSubtitleUseCase
    {
        private readonly ISubtitleRepository _repository;
        private readonly ISubtitleLikeRepository _likeRepository;


        public DeleteSubtitleUseCase(ISubtitleRepository repository, ISubtitleLikeRepository likeRepository)
        {
            _repository = repository;
            _likeRepository = likeRepository;
        }

        public async Task<bool> ExecuteAsync(int subtitleId, string userId)
        {
            var subtitle = await _repository.GetUserSubtitleAsync(subtitleId);
            if (subtitle == null) return false;
            if (subtitle.UserId != userId) return false;

            await _likeRepository.DeleteAllLikesTiedToSubtitleAsync(subtitleId);

            await _repository.DeleteAsync(subtitle);
            return true;
        }
    }
}
