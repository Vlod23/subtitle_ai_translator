using SubtitlesTranslator.Application.DTOs;
using SubtitlesTranslator.Application.Interfaces;
using SubtitlesTranslator.Domain.Enums;

namespace SubtitlesTranslator.Application.UseCases {
    public class CreateDummyRecordUseCase 
    {
        private readonly ISubtitleRepository _subtitleRepository;


        public CreateDummyRecordUseCase(ISubtitleRepository subtitleRepository) {
            _subtitleRepository = subtitleRepository;
        }

        public async Task<int> ExecuteAsync(DummyUploadRequest request) 
        {
            if (string.IsNullOrWhiteSpace(request.SubtitleName))
                throw new ArgumentException("Subtitle name is required");

            var subtitle = new SubtitleTranslation {
                Name = request.SubtitleName,
                Year = request.Year,
                Director = request.Director,
                OriginalText = request.OriginalText,
                TranslatedText = "Translating...",
                TargetLanguage = request.TargetLanguage,
                IsPublic = request.IsPublic,
                UserId = request.User.Id,
                CreditsUsed = request.CreditsNeeded,
                Status = TranslationStatus.Pending
            };
            
            await _subtitleRepository.SaveAsync(subtitle);

            return subtitle.Id;
        }

        public async Task<string> ReadFileTextAsync(IFormFile file) 
        {
            using var reader = new StreamReader(file.OpenReadStream());
            return await reader.ReadToEndAsync();
        }
    }
}
