//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using SubtitlesTranslator.Application.Interfaces;
//using SubtitlesTranslator.Application.UseCases;
//using SubtitlesTranslator.Data;
//using SubtitlesTranslator.Models;

//namespace SubtitlesTranslator.Controllers
//{
//    public class SubtitleQueryController : BaseController
//    {
//        private readonly GetUserDashboardDataUseCase _getUserDashboardData;
//        private readonly IUserSubtitleService _queryService;

//        public SubtitleQueryController(
//            GetUserDashboardDataUseCase getUserDashboardData,
//            IUserSubtitleService queryService, 
//            IUserContextService userContext) 
//            : base(userContext)
//        {
//            _getUserDashboardData = getUserDashboardData;
//            _queryService = queryService;
//        }

//        [Authorize]
//        public async Task<IActionResult> MyTranslations()
//        {
//            var userId = await _userContext.GetCurrentUserIdAsync();
//            var dashboardViewModel = await _getUserDashboardData.ExecuteAsync(userId);
//            if (dashboardViewModel.Subtitles == null)
//                return NotFound();

//            return View(dashboardViewModel);
//        }

//        [Authorize]
//        public async Task<IActionResult> TranslationDetail(int id)
//        {
//            var userId = await _userContext.GetCurrentUserIdAsync();
//            var translationDetailViewModel = await _queryService.GetUserSubtitleDetailsAsync(id, userId);
//            if (translationDetailViewModel == null)
//                return NotFound();

//            return View(translationDetailViewModel);
//        }
//    }
//}
