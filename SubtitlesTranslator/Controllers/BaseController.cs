using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SubtitlesTranslator.Application.Interfaces;
using SubtitlesTranslator.Data;

namespace SubtitlesTranslator.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly IUserContextService _userContext;

        protected BaseController(IUserContextService userContext)
        {
            _userContext = userContext;
        }
    }
}
