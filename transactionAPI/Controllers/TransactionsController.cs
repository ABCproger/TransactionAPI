using CsvHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Formats.Asn1;
using System.Globalization;
using transactionAPI.Data_Transfer_Objects;
using transactionAPI.Entities;
using transactionAPI.Services;
using transactionAPI.Services.Interfaces;

namespace transactionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly TransactionService _transactionService;
        private readonly IExportDataService _exportDataService;

        public TransactionsController(TransactionService transactionService, IExportDataService exportDataService)
        {
            _transactionService = transactionService;
            _exportDataService = exportDataService;
        }
        [HttpPost("import")]
        public async Task<IActionResult> ImportTransactions(IFormFile file)
        {
            if (file.Length == 0)
            {
                return BadRequest("File not selected");
            }
            var requirements = ".csv";
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!requirements.Contains(extension.ToString()))
            {
                return BadRequest("Invalid file extension");
            }
            using var reader = new StreamReader(file.OpenReadStream());
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            var records = csv.GetRecords<TransactionCsvModel>();

            foreach (var record in records)
            {
                if (decimal.TryParse(record.Amount.Replace("$", "").Replace(",", ""), NumberStyles.Number, CultureInfo.InvariantCulture, out var amount))
                {
                    var transaction = new Transaction
                    {
                        TransactionId = record.TransactionId,
                        Name = record.Name,
                        Email = record.Email,
                        Amount = amount,
                        TransactionDate = DateTime.Parse(record.TransactionDate),
                        ClientLocation = record.ClientLocation
                    };

                    if (await _transactionService.TransactionExistsAsync(transaction.TransactionId))
                    {
                        await _transactionService.UpdateTransactionAsync(transaction);
                    }
                    else
                    {
                        await _transactionService.InsertTransactionAsync(transaction);
                    }
                }
            }

            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> GetTransactions()
        {
            var transactions = await _transactionService.GetAllTransactionsAsync();
            return Ok(transactions);
        }
        [HttpGet("export")]
        public async Task<IActionResult> ExportTransactions()
        {
            var transactions = await _transactionService.GetAllTransactionsAsync();
            var excelFile = await _exportDataService.ExportTransactionsToExcelAsync(transactions);

            return File(excelFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Transactions.xlsx");
        }
    }
}
