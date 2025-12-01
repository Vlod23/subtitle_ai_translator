using System;
using System.ComponentModel.DataAnnotations;
using SubtitlesTranslator.Domain.Entities;
using SubtitlesTranslator.Domain.Enums;
using static System.Net.Mime.MediaTypeNames;

public class SubtitleTranslation
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public string? Director { get; set; }

    public int? Year { get; set; }

    [Required]
    public string TargetLanguage { get; set; }

    [Required]
    public string OriginalText { get; set; }

    [Required]
    public string? TranslatedText { get; set; }

    public bool IsPublic { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string UserId { get; set; }
    public ApplicationUser User { get; set; } // <-- navigation property (not creating a new table column, but creating a relationship between tables)

    public int CreditsUsed { get; set; }
    public TranslationStatus Status { get; set; } = TranslationStatus.Pending;
    public ICollection<SubtitleLike> Likes { get; set; } = new List<SubtitleLike>();


    public void SetTranslationSuccess(string text)
    {
        TranslatedText = text;
        Status = TranslationStatus.Translated;
    }

    public void SetTranslationFailure() {
        TranslatedText = "Sorry, the translation failed. But we refunded your credits, so feel free to try again!";
        Status = TranslationStatus.Failed;
    }
}
