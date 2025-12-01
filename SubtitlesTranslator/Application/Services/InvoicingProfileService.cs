using SubtitlesTranslator.Application.Interfaces;
using SubtitlesTranslator.Domain.Entities;
using SubtitlesTranslator.Models;

namespace SubtitlesTranslator.Application.Services {
    public class InvoicingProfileService : IInvoicingProfileService {
        private readonly IUserRepository _userRepository;
        private readonly IInvoicingProfileRepository _invoicingProfileRepository;
        public InvoicingProfileService(
            IUserRepository userRepository,
            IInvoicingProfileRepository invoicingProfileRepository)
        {
            _userRepository = userRepository;
            _invoicingProfileRepository = invoicingProfileRepository;
        }

        public async Task<InvoicingProfileViewModel?> GetAsync(string userId) 
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null) return null;

            var profile = await _invoicingProfileRepository.GetByUserIdAsync(userId);

            var result = new InvoicingProfileViewModel() 
            {
                UserId = user.Id,
                UserName = user.UserName,
                UserEmail = user.Email
            };

            if (profile == null)
            {
                result.CompanyName = string.Empty;
                result.TaxIdentifier = string.Empty;
                result.AddressLine1 = string.Empty;
                result.HouseNumber = string.Empty;
                result.City = string.Empty;
                result.PostalCode = string.Empty;
                result.Country = string.Empty;
            }
            else 
            {
                result.CompanyName = profile.CompanyName;
                result.TaxIdentifier = profile.TaxIdentifier;
                result.AddressLine1 = profile.AddressLine1;
                result.HouseNumber = profile.HouseNumber;
                result.City = profile.City;
                result.PostalCode = profile.PostalCode;
                result.Country = profile.Country;
            }

            return result;
        }

        public async Task SaveAsync(InvoicingProfileViewModel viewModel)
        {
            var invoicingProfile = await _invoicingProfileRepository.GetByUserIdAsync(viewModel.UserId);

            // If profile doesn't exist, create a new one
            if (invoicingProfile == null)
            {
                invoicingProfile = new InvoicingProfile { UserId = viewModel.UserId };
            }

            invoicingProfile.CompanyName = viewModel.CompanyName ?? string.Empty;
            invoicingProfile.TaxIdentifier = viewModel.TaxIdentifier ?? string.Empty;
            invoicingProfile.AddressLine1 = viewModel.AddressLine1 ?? string.Empty;
            invoicingProfile.HouseNumber = viewModel.HouseNumber ?? string.Empty;
            invoicingProfile.City = viewModel.City ?? string.Empty;
            invoicingProfile.PostalCode = viewModel.PostalCode ?? string.Empty;
            invoicingProfile.Country = viewModel.Country ?? string.Empty;

            await _invoicingProfileRepository.SaveAsync(invoicingProfile);
        }
    }
}
