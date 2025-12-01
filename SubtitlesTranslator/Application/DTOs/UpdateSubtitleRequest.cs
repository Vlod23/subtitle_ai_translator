using System.ComponentModel.DataAnnotations;

namespace SubtitlesTranslator.Application.DTOs {
    public class UpdateSubtitleRequest 
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }
        public string? Director { get; set; }
        public int? Year { get; set; }
        public bool IsPublic { get; set; }

        [Required]
        public string? TranslatedText { get; set; }
    }
}
