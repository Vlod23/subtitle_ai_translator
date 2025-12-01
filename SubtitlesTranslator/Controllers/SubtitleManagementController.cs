using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SubtitlesTranslator.Application.DTOs;
using SubtitlesTranslator.Application.Interfaces;
using SubtitlesTranslator.Application.UseCases;
using SubtitlesTranslator.Data;
using SubtitlesTranslator.Domain.Entities;

namespace SubtitlesTranslator.Controllers
{
    public class SubtitleManagementController : BaseController
    {
        private readonly ToggleSubtitleVisibilityUseCase _togglePublicleUseCase;
        private readonly ToggleSubtitleLikeUseCase _toggleLikeUseCase;
        private readonly DeleteSubtitleUseCase _deleteUseCase;
        private readonly UpdateSubtitleUseCase _updateUseCase;
        private readonly GetUserDashboardDataUseCase _getUserDashboardData;
        private readonly IUserSubtitleService _queryService;

        public SubtitleManagementController(
                ToggleSubtitleVisibilityUseCase togglePublicleUseCase,
                ToggleSubtitleLikeUseCase toggleLikeUseCase,
                DeleteSubtitleUseCase deleteUseCase,
                UpdateSubtitleUseCase updateUseCase,
                GetUserDashboardDataUseCase getUserDashboardData,
                IUserSubtitleService queryService,
                IUserContextService userContext) 
            : base(userContext)
        {
            _togglePublicleUseCase = togglePublicleUseCase;
            _toggleLikeUseCase = toggleLikeUseCase;
            _deleteUseCase = deleteUseCase;
            _updateUseCase = updateUseCase;
            _getUserDashboardData = getUserDashboardData;
            _queryService = queryService;
        }

        [Authorize]
        public async Task<IActionResult> MyTranslations() {
            var userId = await _userContext.GetCurrentUserIdAsync();
            var dashboardViewModel = await _getUserDashboardData.ExecuteAsync(userId);
            if (dashboardViewModel.Subtitles == null)
                return NotFound();

            return View(dashboardViewModel);
        }

        [Authorize]
        public async Task<IActionResult> TranslationDetail(int id) {
            var userId = await _userContext.GetCurrentUserIdAsync();
            var translationDetailViewModel = await _queryService.GetUserSubtitleDetailsAsync(id, userId);
            if (translationDetailViewModel == null)
                return NotFound();

            return View(translationDetailViewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TogglePublic(int id, bool isPublic)
        {
            var userId = await _userContext.GetCurrentUserIdAsync();
            var success = await _togglePublicleUseCase.ExecuteAsync(id, userId, isPublic);
            if (!success) return NotFound();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> ToggleLike(int subtitleId) 
        {
            if (User.Identity.IsAuthenticated) {

                var userId = await _userContext.GetCurrentUserIdAsync();
                var newScore = await _toggleLikeUseCase.ExecuteAsync(userId, subtitleId);

                return Json(new { success = true, score = newScore });

            } else 
            {
                return Json(new { success = false, message = "Log in to like subtitles" });
            }
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = await _userContext.GetCurrentUserIdAsync();
            var success = await _deleteUseCase.ExecuteAsync(id, userId);
            if (!success) return NotFound();

            return RedirectToAction("MyTranslations", "SubtitleManagement");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(TranslationDetailViewModel model) 
        {
            if (ModelState.IsValid) 
            {
                var result = await _updateUseCase.ExecuteAsync(new UpdateSubtitleRequest 
                {
                    Id = model.Id,
                    Name = model.Name,
                    Year = model.Year,
                    Director = model.Director,
                    TranslatedText = model.TranslatedText,
                    IsPublic = model.IsPublic,
                });

                TempData["SuccessMsg"] = "Changes saved";
            } 
            else 
            {
                var errorMessages = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .Where(msg => !string.IsNullOrWhiteSpace(msg))
                    .ToList();

                TempData["ErrorMsg"] = string.Join("<br>", errorMessages);
            }

            return RedirectToAction("TranslationDetail", "SubtitleManagement", new { id = model.Id });
        }
    }
}
