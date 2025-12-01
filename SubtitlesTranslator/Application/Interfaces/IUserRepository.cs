namespace SubtitlesTranslator.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<ApplicationUser?> GetUserByIdAsync(string userId);
        Task<bool> UsernameExistsAsync(string userName);
        Task UpdateUserAsync(ApplicationUser user);
    }
}
