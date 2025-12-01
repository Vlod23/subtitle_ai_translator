namespace SubtitlesTranslator.Application.DTOs {
    public class BackgroundTranslateRequest 
    {
        public SubtitleTranslation DummyRecord { get; set; }
        public ApplicationUser User { get; set; }
        public string Prompt { get; set; }
    }
}
