using Dapper;
using Microsoft.Data.SqlClient;
using transactionAPI.Entities;

namespace transactionAPI.Services
{
    public class TransactionService
    {
        private readonly string _connectionString;

        public TransactionService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SqlServerConnection");
        }

        public async Task<bool> TransactionExistsAsync(string transactionId)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "SELECT COUNT(1) FROM Transactions WHERE transaction_id = @TransactionId";
            var result = await connection.ExecuteScalarAsync<int>(sql, new { TransactionId = transactionId });
            return result > 0;
        }

        public async Task UpdateTransactionAsync(Transaction transaction)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"
            UPDATE Transactions 
            SET name = @Name, 
                email = @Email, 
                amount = @Amount, 
                transaction_date = @TransactionDate, 
                client_location = @ClientLocation 
            WHERE transaction_id = @TransactionId";
            await connection.ExecuteAsync(sql, transaction);
        }

        public async Task InsertTransactionAsync(Transaction transaction)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"
            INSERT INTO Transactions (transaction_id, name, email, amount, transaction_date, client_location)
            VALUES (@TransactionId, @Name, @Email, @Amount, @TransactionDate, @ClientLocation)";
            await connection.ExecuteAsync(sql, transaction);
        }
        public async Task<IEnumerable<Transaction>> GetAllTransactionsAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            const string sql = "SELECT transaction_id, name, email, amount, transaction_date, client_location FROM Transactions";
            var result = await connection.QueryAsync(sql);

            var transactions = result.Select(row => new Transaction
            {
                TransactionId = row.transaction_id,
                Name = row.name,
                Email = row.email,
                Amount = row.amount,
                TransactionDate = row.transaction_date,
                ClientLocation = row.client_location
            });

            return transactions;
        }

    }
}
