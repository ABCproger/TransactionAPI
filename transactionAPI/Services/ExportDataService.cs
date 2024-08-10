using OfficeOpenXml;
using transactionAPI.Services.Interfaces;

namespace transactionAPI.Services
{
    public class ExportDataService : IExportDataService
    {
        public async Task<byte[]> ExportTransactionsToExcelAsync<T>(IEnumerable<T> items)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add(typeof(T).Name + "s");

            var properties = typeof(T).GetProperties();

            for (var i = 0; i < properties.Length; i++)
            {
                worksheet.Cells[1, i + 1].Value = properties[i].Name;
            }

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

            return await Task.FromResult(package.GetAsByteArray());
        }

    }
}
