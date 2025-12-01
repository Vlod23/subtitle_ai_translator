using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SubtitlesTranslator.Application.Interfaces;
using SubtitlesTranslator.Data;

namespace SubtitlesTranslator.Infrastructure.Repositories
{
    public class EFUserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public EFUserRepository(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<ApplicationUser?> GetUserByIdAsync(string userId) {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<bool> UsernameExistsAsync(string userName)
        {
            var existingUser = await _userManager.FindByNameAsync(userName);
            return existingUser != null && existingUser.NormalizedUserName == userName.ToUpper();
        }
        public async Task UpdateUserAsync(ApplicationUser user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
