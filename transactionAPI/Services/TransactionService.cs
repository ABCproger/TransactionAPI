using Dapper;
using Microsoft.Data.SqlClient;
using NodaTime;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using transactionAPI.Data_Transfer_Objects;
using transactionAPI.Entities;
using transactionAPI.Services.Interfaces;

namespace transactionAPI.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly string _connectionString;

        public TransactionService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        /// <summary>
        /// Checks if a transaction exists in the database by its ID.
        /// </summary>
        /// <param name="transactionId">The ID of the transaction to check.</param>
        /// <returns>True if the transaction exists; otherwise, false.</returns>
        public async Task<bool> TransactionExistsAsync(string transactionId)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            const string sql = @"SELECT COUNT(1) FROM ""Transactions"" WHERE transaction_id = @TransactionId";
            var result = await connection.ExecuteScalarAsync<int>(sql, new { TransactionId = transactionId });
            return result > 0;
        }

        /// <summary>
        /// Updates an existing transaction in the database.
        /// </summary>
        /// <param name="transaction">The transaction entity containing updated information.</param>
        /// <returns>Task</returns>
        public async Task UpdateTransactionAsync(Transaction transaction)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            const string sql = @"
            UPDATE ""Transactions""
            SET name = @Name, 
                email = @Email, 
                amount = @Amount, 
                transaction_date_local = @TransactionDate, 
                transaction_date_utc = @TransactionDateUtc, 
                client_location = @ClientLocation,
                time_zone_rules = @TimeZoneRules
            WHERE transaction_id = @TransactionId";

            var parameters = new
            {
                transaction.Name,
                transaction.Email,
                transaction.Amount,
                TransactionDate = transaction.TransactionDate.ToDateTimeUnspecified(),
                TransactionDateUtc = transaction.TransactionDateUtc.ToDateTimeUtc(),
                transaction.ClientLocation,
                transaction.TimeZoneRules,
                transaction.TransactionId
            };

            await connection.ExecuteAsync(sql, parameters);
        }

        /// <summary>
        /// Inserts a new transaction into the database.
        /// </summary>
        /// <param name="transaction">The transaction entity to insert.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task InsertTransactionAsync(Transaction transaction)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            const string sql = @"
            INSERT INTO ""Transactions""
            (transaction_id, name, email, amount, transaction_date_local, transaction_date_utc, time_zone_id, client_location, time_zone_rules)
            VALUES (@TransactionId, @Name, @Email, @Amount, @TransactionDate, @TransactionDateUtc, @TimeZoneId, @ClientLocation, @TimeZoneRules)";

            var parameters = new
            {
                transaction.TransactionId,
                transaction.Name,
                transaction.Email,
                transaction.Amount,
                TransactionDate = transaction.TransactionDate.ToDateTimeUnspecified(),
                TransactionDateUtc = transaction.TransactionDateUtc.ToDateTimeUtc(),
                transaction.TimeZoneId,
                transaction.ClientLocation,
                transaction.TimeZoneRules,
            };

            await connection.ExecuteAsync(sql, parameters);
        }

        /// <summary>
        /// Retrieves all transactions from the database.
        /// </summary>
        /// <returns>An enumerable collection of <see cref="TransactionDTO"/> representing the transactions.</returns>
        public async Task<IEnumerable<TransactionDTO>> GetAllTransactionsAsync()
        {
            using var connection = new NpgsqlConnection(_connectionString);
            const string sql = @"
            SELECT transaction_id, name, email, amount, transaction_date_local, time_zone_id, transaction_date_utc, client_location, time_zone_rules 
            FROM ""Transactions""";

            var result = await connection.QueryAsync(sql);

            var transactions = result.Select(row => new TransactionDTO
            {
                TransactionId = row.transaction_id,
                Name = row.name,
                Email = row.email,
                Amount = row.amount,
                TransactionDate = row.transaction_date_local.ToDateTimeUnspecified(),
                TimeZoneId = row.time_zone_id,
                TransactionDateUtc = row.transaction_date_utc.ToDateTimeUtc(),
                ClientLocation = row.client_location,
                TimeZoneRules = row.time_zone_rules,
            });

            return transactions;
        }

        /// <summary>
        /// Retrieves transactions that occurred between the specified dates and match the given time zone ID.
        /// </summary>
        /// <param name="startDate">The start date of the range.</param>
        /// <param name="endDate">The end date of the range.</param>
        /// <param name="timeZoneId">The time zone ID to filter the transactions by.</param>
        /// <returns>An enumerable collection of <see cref="TransactionDTO"/> representing the transactions.</returns>
        public async Task<IEnumerable<TransactionDTO>> GetTransactionsBetweenDates(DateTime startDate, DateTime endDate, string timeZoneId)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            const string sql = @"
            SELECT transaction_id, name, email, amount, transaction_date_local, time_zone_id, transaction_date_utc, client_location, time_zone_rules 
            FROM ""Transactions"" 
            WHERE transaction_date_utc BETWEEN @StartDate AND @EndDate 
            AND time_zone_id = @TimeZoneId";

            var parameters = new
            {
                StartDate = startDate,
                EndDate = endDate,
                TimeZoneId = timeZoneId.Trim()
            };

            var result = await connection.QueryAsync(sql, parameters);

            var transactions = result.Select(row => new TransactionDTO
            {
                TransactionId = row.transaction_id,
                Name = row.name,
                Email = row.email,
                Amount = row.amount,
                TransactionDate = row.transaction_date_local.ToDateTimeUnspecified(),
                TimeZoneId = row.time_zone_id,
                TransactionDateUtc = row.transaction_date_utc.ToDateTimeUtc(),
                ClientLocation = row.client_location,
                TimeZoneRules = row.time_zone_rules,
            });

            return transactions;
        }

        /// <summary>
        /// Retrieves transactions that occurred between the specified dates, without filtering by time zone.
        /// </summary>
        /// <param name="startDate">The start date of the range.</param>
        /// <param name="endDate">The end date of the range.</param>
        /// <returns>An enumerable collection of <see cref="TransactionDTO"/> representing the transactions.</returns>
        public async Task<IEnumerable<TransactionDTO>> GetTransactionsBetweenDates(DateTime startDate, DateTime endDate)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            const string sql = @"
            SELECT transaction_id, name, email, amount, transaction_date_local, time_zone_id, transaction_date_utc, client_location, time_zone_rules 
            FROM ""Transactions"" 
            WHERE transaction_date_local BETWEEN @StartDate AND @EndDate";

            var parameters = new
            {
                StartDate = startDate,
                EndDate = endDate,
            };

            var result = await connection.QueryAsync(sql, parameters);

            var transactions = result.Select(row => new TransactionDTO
            {
                TransactionId = row.transaction_id,
                Name = row.name,
                Email = row.email,
                Amount = row.amount,
                TransactionDate = row.transaction_date_local.ToDateTimeUnspecified(),
                TimeZoneId = row.time_zone_id,
                TransactionDateUtc = row.transaction_date_utc.ToDateTimeUtc(),
                ClientLocation = row.client_location,
                TimeZoneRules = row.time_zone_rules,
            });

            return transactions;
        }
    }
}