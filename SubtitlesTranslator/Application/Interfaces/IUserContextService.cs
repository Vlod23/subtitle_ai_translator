namespace SubtitlesTranslator.Application.Interfaces {
    public interface IUserContextService 
    {
        Task<ApplicationUser> GetCurrentUserAsync();
        Task<string> GetCurrentUserIdAsync();
        Task<string> GetUserStripeIdAsync();
    }
}
