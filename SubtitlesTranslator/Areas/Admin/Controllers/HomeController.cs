using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SubtitlesTranslator.Application.Authorization;

namespace SubtitlesTranslator.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = PermissionNames.ViewAdminArea)]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
