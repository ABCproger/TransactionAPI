using NodaTime;
using transactionAPI.Data_Transfer_Objects;
using transactionAPI.Entities;

namespace transactionAPI.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<bool> TransactionExistsAsync(string transactionId);
        Task UpdateTransactionAsync(Transaction transaction);
        Task InsertTransactionAsync(Transaction transaction);
        Task<IEnumerable<TransactionDTO>> GetAllTransactionsAsync();
        Task<IEnumerable<TransactionDTO>> GetTransactionsBetweenDates(DateTime startDate, DateTime endDate, string timeZoneId);
        Task<IEnumerable<TransactionDTO>> GetTransactionsBetweenDates(DateTime startDate, DateTime endDate);
    }

}
