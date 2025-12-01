using SubtitlesTranslator.Application.Interfaces;

namespace SubtitlesTranslator.Application.UseCases {
    public class SaveStripeCustomerIdUseCase 
    {
        private readonly IUserRepository _userRepository;

        public SaveStripeCustomerIdUseCase(IUserRepository userRepository) {
            _userRepository = userRepository;
        }

        public async Task ExecuteAsync(string userId, string stripeCustomerId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            user.StripeCustomerId = stripeCustomerId;
            await _userRepository.UpdateUserAsync(user);
        }
    }
}
