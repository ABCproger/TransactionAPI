using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Formats.Asn1;
using System.Globalization;

namespace transactionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        [HttpPost("import")]
        public async Task<IActionResult> ImportTransactions(IFormFile file)
        {
            var requirements = ".csv";
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!requirements.Contains(extension.ToString()))
            {
                return BadRequest();
            }
            //using (var reader = new StreamReader(file.OpenReadStream()))
            //using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            //{
            //    var records = csv.GetRecords<Transaction>().ToList();

            //    foreach (var record in records)
            //    {
            //        var existingTransaction = await _context.Transactions
            //            .FirstOrDefaultAsync(t => t.TransactionId == record.TransactionId);

            //        if (existingTransaction == null)
            //        {
            //            _context.Transactions.Add(record);
            //        }
            //        else
            //        {
            //            existingTransaction.Status = record.Status;
            //            existingTransaction.Amount = record.Amount;
            //            existingTransaction.TransactionDate = record.TransactionDate;
            //            existingTransaction.Location = record.Location;
            //            existingTransaction.ClientTimeZone = record.ClientTimeZone;
            //        }
            //    }

            //    await _context.SaveChangesAsync();
            //}

            return Ok();
        }
    }
}
