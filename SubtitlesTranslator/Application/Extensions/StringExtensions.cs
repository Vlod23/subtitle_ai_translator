using Microsoft.AspNetCore.Identity;

namespace SubtitlesTranslator.Application.Extensions
{
    public static class StringExtensions
    {
        // "this" keyword allows this method to be called on any IdentityResult object
        public static string ListAllErrors(this IdentityResult result)
        {
            return string.Join("<br/>", result.Errors.Select(e => e.Description));
        }
    }
}
