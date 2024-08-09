using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NodaTime;
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
                entity.Property(e => e.TransactionId)
                      .HasColumnName("transaction_id");
                entity.Property(e => e.Name)
                      .HasColumnName("name");
                entity.Property(e => e.Email)
                      .HasColumnName("email");
                entity.Property(e => e.Amount)
                      .HasColumnName("amount");
                entity.Property(e => e.TransactionDate)
                      .HasColumnName("transaction_date_local")
                      .HasColumnType("timestamp without time zone");
                entity.Property(e => e.TimeZoneId)
                      .HasColumnName("time_zone_id");
                entity.Property(e => e.ClientLocation)
                      .HasColumnName("client_location");
                entity.Property(e => e.TransactionDateUtc)
                      .HasColumnName("transaction_date_utc")
                      .HasColumnType("timestamp with time zone");
                entity.Property(e => e.TimeZoneRules)
                      .HasColumnName("time_zone_rules");
            });
        }

    }
}
