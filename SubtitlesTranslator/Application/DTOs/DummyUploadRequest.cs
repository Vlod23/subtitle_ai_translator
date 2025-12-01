namespace SubtitlesTranslator.Application.DTOs
{
    public class DummyUploadRequest
    {
        public string SubtitleName { get; set; }
        public string? Director { get; set; }
        public int? Year { get; set; }
        public string TargetLanguage { get; set; }
        public bool IsPublic { get; set; }
        public ApplicationUser User { get; set; }
        public IFormFile File { get; set; }
        public string OriginalText { get; set; }
        public int CreditsNeeded { get; set; }
    }
}
