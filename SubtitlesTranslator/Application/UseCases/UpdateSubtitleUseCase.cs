using SubtitlesTranslator.Application.DTOs;
using SubtitlesTranslator.Application.Interfaces;

namespace SubtitlesTranslator.Application.UseCases {
    public class UpdateSubtitleUseCase 
    {
        private readonly ISubtitleRepository _repository;

        public UpdateSubtitleUseCase(ISubtitleRepository repository) {
            _repository = repository;
        }

        public async Task<UpdateSubtitleResult> ExecuteAsync(UpdateSubtitleRequest request) 
        {
            var subtitle = await _repository.GetUserSubtitleAsync(request.Id);

            if (subtitle != null) {
                subtitle.Id = request.Id;
                subtitle.Name = request.Name;
                subtitle.Year = request.Year;
                subtitle.Director = request.Director;
                subtitle.TranslatedText = request.TranslatedText;
                subtitle.IsPublic = request.IsPublic;

                await _repository.UpdateAsync(subtitle);
            }

            return new UpdateSubtitleResult { SubtitleId = subtitle.Id };
        }
    }
}
