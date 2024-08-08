using NodaTime;
using GeoTimeZone;
using transactionAPI.Services.Interfaces;

namespace transactionAPI.Services
{
    public class TimeZoneService : ITimeZoneService
    {
        private string GetTimeZoneId(double latitude, double longitude)
        {
            var timeZoneResult = TimeZoneLookup.GetTimeZone(latitude, longitude);
            return timeZoneResult.Result;
        }

        private DateTimeZone GetDateTimeZone(double latitude, double longitude)
        {
            var timeZoneId = GetTimeZoneId(latitude, longitude);
            return DateTimeZoneProviders.Tzdb[timeZoneId];
        }

        public ZonedDateTime ConvertToZonedDateTime(DateTime dateTime, double latitude, double longitude)
        {
            var dateTimeZone = GetDateTimeZone(latitude, longitude);
            var instant = Instant.FromDateTimeUtc(DateTime.SpecifyKind(dateTime, DateTimeKind.Utc));
            return instant.InZone(dateTimeZone);
        }
    }
}
