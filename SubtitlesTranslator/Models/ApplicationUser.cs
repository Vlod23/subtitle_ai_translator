using Microsoft.AspNetCore.Identity;
using SubtitlesTranslator.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public int Credits { get; set; }
    public bool IsDeleted { get; set; } = false; // treba dorobiť tzv. soft delete (user sa pri vymazaní nevymaže, ale označí ako vymazaný
    public DateTime? DeletedAt { get; set; } // -//-
    public string? StripeCustomerId { get; set; }

    public InvoicingProfile InvoicingProfile { get; set; }
    public virtual ICollection<PaymentTransaction> PaymentTransactions { get; set; }
    = new List<PaymentTransaction>();
}
