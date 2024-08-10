namespace transactionAPI.Services.Interfaces
{
    public interface IExportDataService
    {
        Task<byte[]> ExportTransactionsToExcelAsync<T>(IEnumerable<T> items);
    }
}
