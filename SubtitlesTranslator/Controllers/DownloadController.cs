using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SubtitlesTranslator.Application.Interfaces;
using SubtitlesTranslator.Application.UseCases;
using SubtitlesTranslator.Data;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SubtitlesTranslator.Controllers
{
    [Authorize]
    public class DownloadController : BaseController
    {
        private readonly DownloadSubtitleUseCase _downloadUseCase;

        public DownloadController(DownloadSubtitleUseCase downloadUseCase,
            IUserContextService userContext)
            : base(userContext)
        {
            _downloadUseCase = downloadUseCase;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> DownloadTranslated(int id, bool? isAuth = true)
        {
            var (content, fileName) = await _downloadUseCase.ExecuteAsync(id);
            if (content == null || fileName == null)
                return NotFound();

            if (isAuth == true) 
            {
                return File(content, "text/plain", fileName);
            } 
            else 
            {
                return RedirectToAction("Index", "Home", new { id = id, reDownload = true });
            }
        }
    }
}
