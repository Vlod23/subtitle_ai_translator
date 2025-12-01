using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SubtitlesTranslator.Application.Interfaces;
using SubtitlesTranslator.Data;
using SubtitlesTranslator.Domain.Entities;

namespace SubtitlesTranslator.Infrastructure.Repositories {
    public class EFInvoicingProfileRepository : IInvoicingProfileRepository 
    {
        private readonly ApplicationDbContext _context;

        public EFInvoicingProfileRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<InvoicingProfile?> GetByUserIdAsync(string userId)
        {
            return await _context.InvoicingProfiles.FirstOrDefaultAsync(profile => profile.UserId == userId);
        }

        public async Task SaveAsync(InvoicingProfile profile)
        {
            var existingProfile = await GetByUserIdAsync(profile.UserId);
            if (existingProfile == null)
            {
                await _context.InvoicingProfiles.AddAsync(profile);
            }
            else
            {
                _context.InvoicingProfiles.Update(profile);
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteInvoicingProfile(InvoicingProfile invoicingProfile)
        {
            if (invoicingProfile != null) {
                _context.InvoicingProfiles.Remove(invoicingProfile);
                await _context.SaveChangesAsync();
            }
        }
    }
}
