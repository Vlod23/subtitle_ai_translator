using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SubtitlesTranslator.Application.Extensions;
using SubtitlesTranslator.Application.Interfaces;
using SubtitlesTranslator.Application.UseCases;
using SubtitlesTranslator.Data;

namespace SubtitlesTranslator.Controllers
{
    public class UserController : BaseController
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UpdateUsernameUseCase _updateUsername;

        public UserController(
            SignInManager<ApplicationUser> signInManager,
            UpdateUsernameUseCase updateUsername,
            IUserContextService userContext) 
            : base(userContext) 
        {
            _signInManager = signInManager;
            _updateUsername = updateUsername;
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUserName(string userName, string returnUrl)
        {
            try {
                var userId = await _userContext.GetCurrentUserIdAsync();
                await _updateUsername.ExecuteAsync(userId, userName);

                TempData["SuccessMsg"] = "Username updated successfully.";

                // refresh cookie so the new name shows up immediately
                var user = await _userContext.GetCurrentUserAsync();
                await _signInManager.RefreshSignInAsync(user);
            } 
            catch (InvalidOperationException ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }

            return RedirectToLocal(returnUrl);
        }

        private IActionResult RedirectToLocal(string? returnUrl)
        {
            return !string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl) 
                ? Redirect(returnUrl) 
                : RedirectToAction("Index", "Home");
        }
    }
}
