using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

public class TranslationDetailViewModel
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

    public int CreditsUsed { get; set; }
}
