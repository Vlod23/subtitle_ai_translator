using SubtitlesTranslator.Application.Interfaces;

namespace SubtitlesTranslator.Application.UseCases
{
    public class ToggleSubtitleVisibilityUseCase
    {
        private readonly ISubtitleRepository _repository;

        public ToggleSubtitleVisibilityUseCase(ISubtitleRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> ExecuteAsync(int subtitleId, string userId, bool isPublic)
        {
            var subtitle = await _repository.GetUserSubtitleAsync(subtitleId);
            if (subtitle == null) return false;

            subtitle.IsPublic = isPublic;
            await _repository.SaveChangesAsync();
            return true;
        }
    }
}
