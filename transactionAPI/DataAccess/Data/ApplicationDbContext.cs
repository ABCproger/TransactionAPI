using Microsoft.EntityFrameworkCore;
using System.Xml;
using transactionAPI.Entities;

namespace transactionAPI.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Transaction> Transactions { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(e => e.TransactionId);
                entity.Property(e => e.TransactionId).HasColumnName("transaction_id");
                entity.Property(e => e.Name).HasColumnName("name");
                entity.Property(e => e.Email).HasColumnName("email");
                entity.Property(e => e.Amount).HasColumnName("amount");
                entity.Property(e => e.TransactionDate).HasColumnName("transaction_date");
                entity.Property(e => e.ClientLocation).HasColumnName("client_location");
            });
        }

    }
}
