using System.Data;
using Microsoft.EntityFrameworkCore;
using SubtitlesTranslator.Application.Interfaces;
using SubtitlesTranslator.Data;
using SubtitlesTranslator.Domain.Entities;

namespace SubtitlesTranslator.Infrastructure.Repositories {
    public class EFInvoiceCounterRepository : IInvoiceCounterRepository
    {
                private readonly ApplicationDbContext _dbContext;

        public EFInvoiceCounterRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> GetNextInvoiceSequenceAsync(int year) 
        {
            // serializable transaction = lock
            await using var tx = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.Serializable);
            var counter = await _dbContext.Set<InvoiceCounter>().FindAsync(year);

            if (counter == null) 
            {
                counter = new InvoiceCounter { Year = year, LastNumber = 1 };
                _dbContext.Add(counter);
            } 
            else 
            {
                counter.LastNumber++;
                _dbContext.Update(counter);
            }

            await _dbContext.SaveChangesAsync();
            await tx.CommitAsync();

            return counter.LastNumber;
        }

    }
}
