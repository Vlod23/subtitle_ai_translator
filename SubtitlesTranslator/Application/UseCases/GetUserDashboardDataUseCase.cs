using SubtitlesTranslator.Application.DTOs;
using SubtitlesTranslator.Application.Interfaces;
using SubtitlesTranslator.Models;

namespace SubtitlesTranslator.Application.UseCases {
    public class GetUserDashboardDataUseCase 
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserSubtitleService _queryService;

        public GetUserDashboardDataUseCase(IUserRepository userRepo, IUserSubtitleService queryService) 
        {
            _userRepository = userRepo;
            _queryService = queryService;            
        }

        public async Task<UserDashboardViewModel> ExecuteAsync(string userId) 
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            var subtitles = await _queryService.GetUserSubtitlesAsync(user.Id);

            if (user == null) return null;

            return new UserDashboardViewModel {
                UserName = user.UserName,
                Credits = user.Credits,
                Subtitles = subtitles
            };
        }
    }

}
