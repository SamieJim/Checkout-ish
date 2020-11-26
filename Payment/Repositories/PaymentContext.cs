using Payment.Models;
using Microsoft.EntityFrameworkCore;

namespace Payment.Repositories
{
    public class PaymentContext : DbContext
    {
        public PaymentContext(DbContextOptions<PaymentContext> opt) : base(opt)
        {
            
        }

        public DbSet<Transaction> Transaction { get; set; }

    }
}