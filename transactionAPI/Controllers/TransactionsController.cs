using CsvHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using NodaTime.Extensions;
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
        private readonly ITransactionService _transactionService;
        private readonly IExportDataService _exportDataService;
        private readonly ITimeZoneService _timeZoneService;

        public TransactionsController(ITransactionService transactionService,
            IExportDataService exportDataService,
            ITimeZoneService timeZoneService)
        {
            _transactionService = transactionService;
            _exportDataService = exportDataService;
            _timeZoneService = timeZoneService;
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
                var location = _timeZoneService.ParseLocation(record.ClientLocation);
                var dateTime = DateTime.Parse(record.TransactionDate);
                var timeZone = _timeZoneService.GetDateTimeZone(location.Latitude, location.Longitude);
               
                var localDateTime = LocalDateTime.FromDateTime(dateTime);
                var utcTime = _timeZoneService.ConvertToUtc(localDateTime, location.Latitude, location.Longitude);
                var tzdbSource = DateTimeZoneProviders.Tzdb;

                string versionId = tzdbSource.VersionId;
                if (decimal.TryParse(record.Amount.Replace("$", "").Replace(",", ""), NumberStyles.Number, CultureInfo.InvariantCulture, out var amount))
                {
                    var transaction = new Transaction
                    {
                        TransactionId = record.TransactionId,
                        Name = record.Name,
                        Email = record.Email,
                        Amount = amount,
                        TransactionDate = localDateTime,
                        TimeZoneId = timeZone.ToString(),
                        ClientLocation = record.ClientLocation,
                        TransactionDateUtc = utcTime,
                        TimeZoneRules = versionId
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
        public async Task<IActionResult> GetTransactions(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            [FromQuery] string? userLatitude,
            [FromQuery] string? userLongtitude)
        {
            if (!double.TryParse(userLatitude, NumberStyles.Any, CultureInfo.InvariantCulture, out var latiTude) ||
       !double.TryParse(userLongtitude, NumberStyles.Any, CultureInfo.InvariantCulture, out var longTitude))
            {
                return BadRequest("Invalid latitude or longitude format.");
            }
            var zonedStartDate = _timeZoneService.ConvertToZonedDateTime(startDate, latiTude, longTitude);
            var zonedEndDate = _timeZoneService.ConvertToZonedDateTime(endDate, latiTude, longTitude);
            var startDateUtc = zonedStartDate.ToDateTimeUtc();
            var endDateUtc = zonedEndDate.ToDateTimeUtc();
            var transactions = await _transactionService.GetTransactionsBetweenDates(startDateUtc, endDateUtc);
            return Ok(transactions);
        }
        [HttpGet("GETTTTTTTTTTt")]
        public async Task<IActionResult> gettrans()
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
