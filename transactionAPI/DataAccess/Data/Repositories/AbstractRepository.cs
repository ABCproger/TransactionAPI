namespace transactionAPI.DataAccess.Data.Repositories
{
    public class AbstractRepository
    {
        protected readonly ApplicationDbContext _context;

        protected AbstractRepository(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}
