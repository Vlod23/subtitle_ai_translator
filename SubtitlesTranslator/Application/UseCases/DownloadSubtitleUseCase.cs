using SubtitlesTranslator.Application.Interfaces;
using System.Text;

namespace SubtitlesTranslator.Application.UseCases
{
    public class DownloadSubtitleUseCase
    {
        private readonly ISubtitleRepository _repository;

        public DownloadSubtitleUseCase(ISubtitleRepository repository)
        {
            _repository = repository;
        }

        public async Task<(byte[]? Content, string? FileName)> ExecuteAsync(int subtitleId)
        {
            var subtitle = await _repository.GetUserSubtitleAsync(subtitleId);

            var content = GetByteFileContent(subtitle);
            var fileName = SetFileName(subtitle.Name);

            return (content, fileName);
        }

        private byte[]? GetByteFileContent(SubtitleTranslation subtitle)
        {
            if (subtitle == null || string.IsNullOrWhiteSpace(subtitle.TranslatedText))
                return null;

            return Encoding.UTF8.GetBytes(subtitle.TranslatedText);
        }

        private string SetFileName(string subtitleName)
        {
            return $"{subtitleName?.Replace(" ", "_")}_translated.srt";
        }
    }
}
