using System.Globalization;
using Microsoft.AspNetCore.Mvc.Rendering;
using SubtitlesTranslator.Application.Interfaces;

namespace SubtitlesTranslator.Application.Services {
    public class LanguageService : ILanguageService
    {
        public List<SelectListItem> GetAllLanguages() {
            return CultureInfo
                .GetCultures(CultureTypes.SpecificCultures)
                .Where(c => !string.IsNullOrEmpty(c.Name)
                         && c.Name != CultureInfo.InvariantCulture.Name)
                .Select(c => new SelectListItem {
                    Value = c.EnglishName,
                    Text = c.EnglishName
                })
                .OrderBy(li => li.Text)
                .ToList();
        }
    }
}
