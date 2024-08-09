using NodaTime;

namespace transactionAPI.Data_Transfer_Objects
{
    public class TransactionDTO
    {

        public string TransactionId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TimeZoneId { get; set; }
        public string ClientLocation { get; set; }
        public DateTime TransactionDateUtc { get; set; }
        public string TimeZoneRules { get; set; }
    }
}
