using NodaTime;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace transactionAPI.Entities
{
    public class Transaction
    {
        [Key]
        [Column("transaction_id")]
        public string TransactionId { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("email")]
        public string Email { get; set; }
        [Column("amount")]
        public decimal Amount { get; set; }
        [Column("transaction_date_local")]
        public LocalDateTime TransactionDate { get; set; }
        [Column("time_zone_id")]
        public string TimeZoneId { get; set; }
        [Column("client_location")]
        public string ClientLocation { get; set; }
        [Column("transaction_date_utc")]
        public Instant TransactionDateUtc { get; set; }
        [Column("time_zone_rules")]
        public string TimeZoneRules { get; set; }
    }

}
