using CsvHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NodaTime;
using NodaTime.Extensions;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Net;
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

        /// <summary>
        /// Imports transactions from a CSV file.
        /// </summary>
        /// <param name="file">The CSV file containing the transactions.</param>
        /// <returns>Action result indicating the status of the import operation.</returns>
        [HttpPost("import")]
        [ProducesResponseType(StatusCodes.Status200OK)]
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

        /// <summary>
        /// Returns a list of transactions in the API user's timezone between the specified dates.
        /// </summary>
        /// <param name="startDate">The start date for filtering transactions in the user's local time.</param>
        /// <param name="endDate">The end date for filtering transactions in the user's local time.</param>
        /// <param name="timeZoneId">The timezone ID of the API user.</param>
        /// <returns>A list of transactions created in the user's timezone between the specified dates.</returns>
        [HttpGet("{timeZoneId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTransactions(
            [FromQuery, Required] DateTime startDate,
            [FromQuery, Required] DateTime endDate,
            string timeZoneId)
        {
            var decodedTimeZoneId = WebUtility.UrlDecode(timeZoneId).Replace(" ", "+");

            if (string.IsNullOrEmpty(decodedTimeZoneId))
            {
                return BadRequest("TimeZoneId is required.");
            }

            if (startDate == DateTime.MinValue || endDate == DateTime.MinValue)
            {
                return BadRequest("StartDate and EndDate cannot be default values.");
            }

            if (startDate > endDate)
            {
                return BadRequest("StartDate cannot be later than EndDate.");
            }

            var timeZone = _timeZoneService.GetDateTimeZone(decodedTimeZoneId);

            var localStartDateTime = LocalDateTime.FromDateTime(startDate);
            var localEndDateTime = LocalDateTime.FromDateTime(endDate);

            var startDateUtc = _timeZoneService.ConvertToUtc(localStartDateTime, timeZone);
            var endDateUtc = _timeZoneService.ConvertToUtc(localEndDateTime, timeZone);

            var transactions = await _transactionService.GetTransactionsBetweenDates(startDateUtc.ToDateTimeUtc(), endDateUtc.ToDateTimeUtc(), decodedTimeZoneId);

            var serializedTransactions = JsonConvert.SerializeObject(transactions, Formatting.Indented);

            return Content(serializedTransactions, "application/json");
        }

        /// <summary>
        /// Returns a list of transactions between the specified dates.
        /// </summary>
        /// <param name="startDate">The start date for filtering transactions.</param>
        /// <param name="endDate">The end date for filtering transactions.</param>
        /// <returns>A list of transactions between the specified dates.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTransactions([FromQuery, Required] DateTime startDate, [FromQuery, Required] DateTime endDate)
        {
            if (startDate == DateTime.MinValue || endDate == DateTime.MinValue)
            {
                return BadRequest("StartDate and EndDate cannot be default values.");
            }

            if (startDate > endDate)
            {
                return BadRequest("StartDate cannot be later than EndDate.");
            }
            var transactions = await _transactionService.GetTransactionsBetweenDates(startDate, endDate);
            var serializedTransactions = JsonConvert.SerializeObject(transactions, Formatting.Indented);

            return Content(serializedTransactions, "application/json");
        }

        /// <summary>
        /// Exports all transactions to an Excel file.
        /// </summary>
        /// <returns>An Excel file containing all transactions.</returns>
        [HttpGet("export")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ExportTransactions()
        {
            var transactions = await _transactionService.GetAllTransactionsAsync();
            var excelFile = await _exportDataService.ExportTransactionsToExcelAsync(transactions);

            return File(excelFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Transactions.xlsx");
        }
    }
}