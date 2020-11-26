using PaymentRetrieve.Models;
using Microsoft.EntityFrameworkCore;

namespace PaymentRetrieve.Repositories
{
    public class PaymentContext : DbContext
    {
        public PaymentContext(DbContextOptions<PaymentContext> opt) : base(opt)
        {
            
        }

        public DbSet<Transaction> Transaction { get; set; }

    }
}