using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

public class SubtitleUploadViewModel
{
    public string? SubtitleName { get; set; }

    public string? Director { get; set; }

    public int? Year { get; set; }

    [Required]
    public string TargetLanguage { get; set; }

    public bool IsPublic { get; set; }

    [Required]
    public IFormFile File { get; set; }
    public int CreditCost { get; set; }
    public List<SelectListItem> LanguageList { get; set; } = new();
}
