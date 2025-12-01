using SubtitlesTranslator.Application.Interfaces;
using SubtitlesTranslator.Models;

namespace SubtitlesTranslator.Application.UseCases
{
    public class GetPublicSubtitlesUseCase
    {
        private readonly ISubtitleRepository _repository;

        public GetPublicSubtitlesUseCase(ISubtitleRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<PublicSubtitlesViewModel>> ExecuteAsync()
        {
            var subtitles = await _repository.GetPublicSubtitlesAsync();

            return subtitles.Select(s => new PublicSubtitlesViewModel
            {
                Id = s.Id,
                Name = s.Name,
                Year = s.Year,
                TargetLanguage = s.TargetLanguage,
                CreatedAt = s.CreatedAt,
                UserName = s.User?.UserName ?? "",
                LikesCount = s.Likes.Count()
            });
        }
    }

}
