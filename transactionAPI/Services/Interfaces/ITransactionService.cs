using NodaTime;
using transactionAPI.Entities;

namespace transactionAPI.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<bool> TransactionExistsAsync(string transactionId);
        Task UpdateTransactionAsync(Transaction transaction);
        Task InsertTransactionAsync(Transaction transaction);
        Task<IEnumerable<Transaction>> GetAllTransactionsAsync();
        Task<IEnumerable<Transaction>> GetTransactionsBetweenDates(DateTime startDate, DateTime endDate);
    }

}
