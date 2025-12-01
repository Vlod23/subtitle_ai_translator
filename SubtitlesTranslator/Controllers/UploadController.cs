using Azure.Core;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using SubtitlesTranslator.Application.DTOs;
using SubtitlesTranslator.Application.Interfaces;
using SubtitlesTranslator.Application.UseCases;
using SubtitlesTranslator.Data;
using SubtitlesTranslator.Infrastructure.Repositories;
using SubtitlesTranslator.Infrastructure.Services;

namespace SubtitlesTranslator.Controllers
{
    [Authorize]
    public class UploadController : BaseController
    {
        private readonly CreateDummyRecordUseCase _createDummyRecord;
        private readonly BackgroundTranslateUseCase _backgroundTranslate;
        private readonly ICreditEstimator _estimator;
        private readonly IChatGptService _chatGpt;
        private readonly ILanguageService _languageService;

        public UploadController(ApplicationDbContext context,
            CreateDummyRecordUseCase createDummyRecord,
            BackgroundTranslateUseCase backgroundTranslate,
            ICreditEstimator estimator,
            IChatGptService chatGpt,
            IUserContextService userContext,
            ILanguageService languageService) 
            : base(userContext)
        {
            _createDummyRecord = createDummyRecord;
            _backgroundTranslate = backgroundTranslate;
            _estimator = estimator;
            _chatGpt = chatGpt;
            _languageService = languageService;
        }

        [HttpGet]
        public IActionResult SubtitleUpload()
        {
            var model = new SubtitleUploadViewModel
            {
                LanguageList = _languageService.GetAllLanguages()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(SubtitleUploadViewModel model)
        {
            // Refill the language list for the case when sent back to the form)
            model.LanguageList = _languageService.GetAllLanguages();

            // Check file emptiness
            if (model.File != null)
            {
                using (var reader = new StreamReader(model.File.OpenReadStream())) 
                {
                    var content = await reader.ReadToEndAsync();
                    if (string.IsNullOrWhiteSpace(content)) 
                    {
                        ModelState.AddModelError("File", "The file can not be empty.");
                        return View("SubtitleUpload", model);
                    }
                }
            }

            var user = await _userContext.GetCurrentUserAsync();

            // Check available credits
            if (user.Credits < model.CreditCost) {
                TempData["ErrorMsg"] = $"Not enough credits. You need {model.CreditCost} credits.";
                return View("SubtitleUpload", model);
            }

            // Set name if empty
            model.SubtitleName = string.IsNullOrEmpty(model.SubtitleName)
                ? Path.GetFileNameWithoutExtension(model.File.FileName) + "_" + model.TargetLanguage
                : model.SubtitleName;

            if (!ModelState.IsValid) {
                TempData["ErrorMsg"] = $"Model is invalid. Something is wrong. If the issue persists, please contact us.";
                return View("SubtitleUpload", model);
            }
              

            // Create dummy record
            var originalText = await _createDummyRecord.ReadFileTextAsync(model.File);
            var subtitleId = await _createDummyRecord.ExecuteAsync(new DummyUploadRequest
            {
                SubtitleName = model.SubtitleName,
                Year = model.Year,
                Director = model.Director,
                File = model.File,
                TargetLanguage = model.TargetLanguage,
                IsPublic = model.IsPublic,
                User = user,
                OriginalText = originalText,
                CreditsNeeded = model.CreditCost,
            });

            // Start translation in the background
            BackgroundJob.Enqueue(() => _backgroundTranslate.ExecuteAsync(subtitleId));

            return RedirectToAction("MyTranslations", "SubtitleManagement");
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EstimateCredits(IFormFile file)
        {
            if (file == null)
                return BadRequest("Missing file.");

            using var reader = new StreamReader(file.OpenReadStream());
            var originalText = await reader.ReadToEndAsync();
            string prompt = _chatGpt.SetPrompt("random"); // send a language?
            int credits = _estimator.EstimateCredits(prompt, originalText);

            return Json(new { credits });
        }
    }
}
