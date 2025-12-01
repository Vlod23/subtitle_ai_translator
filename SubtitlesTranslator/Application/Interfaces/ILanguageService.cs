using Microsoft.AspNetCore.Mvc.Rendering;

namespace SubtitlesTranslator.Application.Interfaces {
    public interface ILanguageService 
    {
        List<SelectListItem> GetAllLanguages();
    }
}
