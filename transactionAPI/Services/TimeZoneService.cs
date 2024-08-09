using NodaTime;
using GeoTimeZone;
using transactionAPI.Services.Interfaces;
using System.Globalization;
using transactionAPI.Data_Transfer_Objects;

namespace transactionAPI.Services
{
    public class TimeZoneService : ITimeZoneService
    {
        private string GetTimeZoneId(double latitude, double longitude)
        {
            var timeZoneResult = TimeZoneLookup.GetTimeZone(latitude, longitude);
            return timeZoneResult.Result;
        }

        public DateTimeZone GetDateTimeZone(double latitude, double longitude)
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
        public Instant ConvertToUtc(LocalDateTime localDateTime, double latitude, double longitude)
        {
            var dateTimeZone = GetDateTimeZone(latitude, longitude);

            var zonedDateTime = localDateTime.InZoneLeniently(dateTimeZone);

            return zonedDateTime.ToInstant();
        }
        public LocationDto ParseLocation(string clientLocation)
        {
            var parts = clientLocation.Split(',');
            if (parts.Length != 2)
            {
                throw new ArgumentException("Invalid location format", nameof(clientLocation));
            }

            if (double.TryParse(parts[0], NumberStyles.Number, CultureInfo.InvariantCulture, out var latitude) &&
                double.TryParse(parts[1], NumberStyles.Number, CultureInfo.InvariantCulture, out var longitude))
            {
                return new LocationDto
                {
                    Latitude = latitude,
                    Longitude = longitude
                };
            }

            throw new ArgumentException("Invalid latitude or longitude values", nameof(clientLocation));
        }
    }
}
