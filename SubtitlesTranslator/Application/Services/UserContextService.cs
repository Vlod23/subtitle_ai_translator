using Microsoft.AspNetCore.Identity;
using SubtitlesTranslator.Application.Interfaces;

namespace SubtitlesTranslator.Application.Services {
    public class UserContextService : IUserContextService {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContextService(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor) 
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApplicationUser> GetCurrentUserAsync() {
            var httpContext = _httpContextAccessor.HttpContext
                              ?? throw new InvalidOperationException("No HTTP context available.");

            var principal = httpContext.User;
            if (principal?.Identity?.IsAuthenticated != true)
                throw new UnauthorizedAccessException("User is not authenticated.");

            var user = await _userManager.GetUserAsync(principal);
            if (user == null)
                throw new InvalidOperationException("Authenticated user not found.");

            return user;
        }

        public async Task<string> GetCurrentUserIdAsync() {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null || httpContext.User == null)
                throw new Exception("No active HTTP context or user.");

            var user = await _userManager.GetUserAsync(httpContext.User);
            if (user == null)
                throw new Exception("User not found in the current context.");

            return user.Id;
        }

        public async Task<string> GetUserStripeIdAsync() 
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null || httpContext.User == null)
                throw new Exception("No active HTTP context or user.");

            var user = await _userManager.GetUserAsync(httpContext.User);
            if (user == null)
                throw new Exception("User not found in the current context.");

            return user.StripeCustomerId ?? throw new Exception("Stripe ID not found");
        }
    }
}
