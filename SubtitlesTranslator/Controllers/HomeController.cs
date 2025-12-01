using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SubtitlesTranslator.Application.UseCases;
using SubtitlesTranslator.Models;

namespace SubtitlesTranslator.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly GetPublicSubtitlesUseCase _getPublicSubtitles;

        public HomeController(ILogger<HomeController> logger, GetPublicSubtitlesUseCase getPublicSubtitles)
        {
            _logger = logger;
            _getPublicSubtitles = getPublicSubtitles;
        }

        public async Task<IActionResult> Index()
        {
            var publicSubtitles = await _getPublicSubtitles.ExecuteAsync();
            return View(publicSubtitles);
        }

        public IActionResult PrivacyPolicy() {
            return View();
        }
        public IActionResult TermsOfUse() {
            return View();
        }
        public IActionResult CookiePolicy() {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
