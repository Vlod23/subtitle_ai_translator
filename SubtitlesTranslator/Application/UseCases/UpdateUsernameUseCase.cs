using SubtitlesTranslator.Application.Interfaces;

namespace SubtitlesTranslator.Application.UseCases {
    public class UpdateUsernameUseCase {
        private readonly IUserRepository _userRepository;

        public UpdateUsernameUseCase(IUserRepository userRepository) 
        {
            _userRepository = userRepository;
        }

        public async Task ExecuteAsync(string userId, string newUserName) 
        {
            if (string.IsNullOrWhiteSpace(newUserName)) throw new InvalidOperationException("Username cannot be empty.");
            if (await _userRepository.UsernameExistsAsync(newUserName)) throw new InvalidOperationException("Username is already taken.");

            var user = await _userRepository.GetUserByIdAsync(userId) ?? throw new InvalidOperationException("User not found.");

            user.UserName = newUserName;
            user.NormalizedUserName = newUserName.ToUpperInvariant();

            await _userRepository.UpdateUserAsync(user);
        }
    }
}
