using Microsoft.EntityFrameworkCore;
using transactionAPI.DataAccess.Data.Interfaces;
using transactionAPI.Entities;

namespace transactionAPI.DataAccess.Data.Repositories
{
    public class TransactionRepository : AbstractRepository, IRepository<Transaction>
    {
        private readonly DbSet<Transaction> dbSet;
        public TransactionRepository(ApplicationDbContext context) : base(context)
        {
            dbSet = context.Set<Transaction>();
        }
        public async Task AddAsync(Transaction entity)
        {
            await dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Transaction>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }

        public async Task<bool> UpdateAsync(Transaction entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var result = await dbSet.FindAsync(entity.Id);

            if (result != null)
            {
                result.TransactionId = entity.TransactionId;
                result.Name = entity.Name;
                result.Email = entity.Email;
                result.Amount = entity.Amount;
                result.TransactionDate = entity.TransactionDate;
                result.ClientLocation = entity.ClientLocation;

                dbSet.Update(result);
                await _context.SaveChangesAsync();
                return true;
            }
            
            return false;
        }
    }
}
