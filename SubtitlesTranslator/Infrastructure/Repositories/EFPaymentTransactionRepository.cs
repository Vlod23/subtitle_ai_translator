using Microsoft.EntityFrameworkCore;
using SubtitlesTranslator.Application.Interfaces;
using SubtitlesTranslator.Data;
using SubtitlesTranslator.Domain.Entities;

namespace SubtitlesTranslator.Infrastructure.Repositories {
    public class EFPaymentTransactionRepository : IPaymentTransactionRepository 
    {
        private readonly ApplicationDbContext _context;
        public EFPaymentTransactionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PaymentTransaction> AddTransactionAsync(PaymentTransaction transaction) 
        {
            _context.PaymentTransactions.Add(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task<List<PaymentTransaction>> GetByUserIdAsync(string userId) 
        {
            return await _context.PaymentTransactions
                .Where(t => t.UserId == userId)
                .ToListAsync();
        }

        public async Task<PaymentTransaction?> GetByIdAsync(int id) 
        {
            return await _context.PaymentTransactions
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<PaymentTransaction?> GetByInvoiceNumberAsync(string invoiceNumber) 
        {
            return await _context.PaymentTransactions
                .FirstOrDefaultAsync(t => t.InvoiceNumber == invoiceNumber);
        }
    }
}
