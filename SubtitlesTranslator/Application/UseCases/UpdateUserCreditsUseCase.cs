using SubtitlesTranslator.Application.Interfaces;

namespace SubtitlesTranslator.Application.UseCases 
{
    public class UpdateUserCreditsUseCase 
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserCreditsUseCase(IUserRepository userRepository) {
            _userRepository = userRepository;
        }

        public async Task ExecuteAsync(string userId, int credits, bool add) 
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user != null) {
                if (add) {
                    user.Credits += credits;
                } else {
                    user.Credits -= credits;
                }
                await _userRepository.UpdateUserAsync(user);
            }
        }
    }
}