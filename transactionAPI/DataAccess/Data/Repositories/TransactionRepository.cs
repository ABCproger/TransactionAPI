using System.Transactions;
using transactionAPI.DataAccess.Data.Interfaces;

namespace transactionAPI.DataAccess.Data.Repositories
{
    public class TransactionRepository : IRepository<Transaction>
    {
        public Task AddAsync(Transaction entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Transaction>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Transaction entity)
        {
            throw new NotImplementedException();
        }
    }
}
