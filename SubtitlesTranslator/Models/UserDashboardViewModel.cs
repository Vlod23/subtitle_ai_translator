using Microsoft.AspNetCore.Identity;
using SubtitlesTranslator.Application.Interfaces;

namespace SubtitlesTranslator.Models
{
    public class UserDashboardViewModel
    {
        public string UserName { get; set; }
        public int Credits { get; set; }
        public List<SubtitleTranslation> Subtitles { get; set; }
    }
}
