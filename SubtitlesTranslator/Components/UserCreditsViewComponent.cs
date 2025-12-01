using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace SubtitlesTranslator.Components
{
    public class UserCreditsViewComponent : ViewComponent
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserCreditsViewComponent(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            return View(user?.Credits ?? 0);
        }
    }
}
