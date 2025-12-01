namespace SubtitlesTranslator.Models
{
    public class PublicSubtitlesViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Year { get; set; }
        public string TargetLanguage { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserName { get; set; }
        public int LikesCount { get; set; }
    }
}
