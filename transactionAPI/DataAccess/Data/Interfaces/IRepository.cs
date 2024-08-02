namespace transactionAPI.DataAccess.Data.Interfaces
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task<bool> UpdateAsync(T entity);
    }
}
