using OfficeOpenXml;
using transactionAPI.Services.Interfaces;

namespace transactionAPI.Services
{
    // <summary>
    /// Implements methods for exporting data to Excel.
    /// </summary>
    public class ExportDataService : IExportDataService
    {
        /// <summary>
        /// Exports a collection of items to an Excel file asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the items in the collection.</typeparam>
        /// <param name="items">The collection of items to be exported.</param>
        /// <returns>The task result is a byte array representing the Excel file.</returns>
        public async Task<byte[]> ExportTransactionsToExcelAsync<T>(IEnumerable<T> items)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using var package = new ExcelPackage();

            var worksheet = package.Workbook.Worksheets.Add(typeof(T).Name + "s");

            // Get properties of the type T
            var properties = typeof(T).GetProperties();

            // Add headers to the first row of the worksheet
            for (var i = 0; i < properties.Length; i++)
            {
                worksheet.Cells[1, i + 1].Value = properties[i].Name;
            }

            // Start adding data from the second row
            var row = 2;
            foreach (var item in items)
            {
                for (var i = 0; i < properties.Length; i++)
                {
                    var value = properties[i].GetValue(item);

                    if (value is DateTime dateTimeValue)
                    {
                        worksheet.Cells[row, i + 1].Value = dateTimeValue.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    }
                    else
                    {
                        worksheet.Cells[row, i + 1].Value = value;
                    }
                }
                row++;
            }

            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

            // Return the Excel file as a byte array
            return await Task.FromResult(package.GetAsByteArray());
        }
    }
}
