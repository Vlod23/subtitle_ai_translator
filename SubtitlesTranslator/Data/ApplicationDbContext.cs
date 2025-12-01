using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SubtitlesTranslator.Domain.Entities;

namespace SubtitlesTranslator.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<SubtitleTranslation> SubtitleTranslations { get; set; }
        public DbSet<SubtitleLike> SubtitleLikes { get; set; }
        public DbSet<InvoicingProfile> InvoicingProfiles { get; set; }
        public DbSet<PaymentTransaction> PaymentTransactions { get; set; }
        public DbSet<InvoiceCounter> InvoiceCounter { get; set; }

        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);

            builder.Entity<SubtitleLike>()
                .HasOne(sl => sl.SubtitleTranslation)
                .WithMany(st => st.Likes)
                .HasForeignKey(sl => sl.SubtitleTranslationId)
                .OnDelete(DeleteBehavior.Restrict); // Too many cascading or something.. It's handled manually in EFSubtitleLikeRepository

            builder.Entity<SubtitleLike>()
                .HasOne(sl => sl.User)
                .WithMany()
                .HasForeignKey(sl => sl.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<InvoicingProfile>(eb =>
            {
                eb.HasKey(p => p.UserId);
                eb.HasOne(p => p.User)
                  .WithOne(u => u.InvoicingProfile)
                  .HasForeignKey<InvoicingProfile>(p => p.UserId)
                  .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<PaymentTransaction>(tb =>
            {
                tb.HasKey(t => t.Id);
                tb.Property(t => t.TransactionDate)
                  .HasDefaultValueSql("GETUTCDATE()");
                tb.Property(t => t.Amount)
                  .HasColumnType("decimal(18,2)");
                tb.HasOne(t => t.User)
                  .WithMany(u => u.PaymentTransactions)
                  .HasForeignKey(t => t.UserId)
                  .OnDelete(DeleteBehavior.Restrict);
                tb.HasIndex(t => t.InvoiceNumber).IsUnique();
            });

            builder.Entity<InvoiceCounter>(eb =>
            {
                eb.HasKey(c => c.Year);
                eb.Property(c => c.Year)
                  .ValueGeneratedNever();
            });
        }
    }
}
