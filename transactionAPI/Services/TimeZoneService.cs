using NodaTime;
using GeoTimeZone;
using transactionAPI.Services.Interfaces;
using System.Globalization;
using transactionAPI.Data_Transfer_Objects;
using NodaTime.TimeZones;
using System;

namespace transactionAPI.Services
{
    /// <summary>
    /// Service for handling time zone-related operations, such as converting times to specific time zones or parsing locations.
    /// </summary>
    public class TimeZoneService : ITimeZoneService
    {
        /// <summary>
        /// Retrieves the time zone ID based on latitude and longitude.
        /// </summary>
        /// <param name="latitude">The latitude of the location.</param>
        /// <param name="longitude">The longitude of the location.</param>
        /// <returns>A time zone ID.</returns>
        private string GetTimeZoneId(double latitude, double longitude)
        {
            var timeZoneResult = TimeZoneLookup.GetTimeZone(latitude, longitude);
            return timeZoneResult.Result;
        }

        /// <summary>
        /// Retrieves the DateTimeZone object for a given latitude and longitude.
        /// </summary>
        /// <param name="latitude">The latitude of the location.</param>
        /// <param name="longitude">The longitude of the location.</param>
        /// <returns>The DateTimeZone by the specified ID</returns>
        public DateTimeZone GetDateTimeZone(double latitude, double longitude)
        {
            var timeZoneId = GetTimeZoneId(latitude, longitude);
            return DateTimeZoneProviders.Tzdb[timeZoneId];
        }

        /// <summary>
        /// Retrieves the DateTimeZone object based on a given time zone ID.
        /// </summary>
        /// <param name="timeZoneId">The time zone ID.</param>
        /// <returns>The DateTimeZone object for the specified ID, or null if the ID is not found.</returns>
        public DateTimeZone GetDateTimeZone(string timeZoneId)
        {
            try
            {
                return DateTimeZoneProviders.Tzdb[timeZoneId];
            }
            catch (DateTimeZoneNotFoundException)
            {
                return null;
            }
        }

        /// <summary>
        /// Converts a given DateTime to a ZonedDateTime using the time zone at the specified location.
        /// </summary>
        /// <param name="dateTime">The DateTime object to convert.</param>
        /// <param name="latitude">The latitude of the location.</param>
        /// <param name="longitude">The longitude of the location.</param>
        /// <returns>A ZonedDateTime object representing the time in the specified time zone.</returns>
        public ZonedDateTime ConvertToZonedDateTime(DateTime dateTime, double latitude, double longitude)
        {
            var dateTimeZone = GetDateTimeZone(latitude, longitude);
            var instant = Instant.FromDateTimeUtc(DateTime.SpecifyKind(dateTime, DateTimeKind.Utc));
            return instant.InZone(dateTimeZone);
        }

        /// <summary>
        /// Converts a LocalDateTime to UTC Instant using the time zone at the specified location.
        /// </summary>
        /// <param name="localDateTime">The LocalDateTime object to convert.</param>
        /// <param name="latitude">The latitude of the location.</param>
        /// <param name="longitude">The longitude of the location.</param>
        /// <returns>An Instant object representing the UTC time.</returns>
        public Instant ConvertToUtc(LocalDateTime localDateTime, double latitude, double longitude)
        {
            var dateTimeZone = GetDateTimeZone(latitude, longitude);
            var zonedDateTime = localDateTime.InZoneLeniently(dateTimeZone);
            return zonedDateTime.ToInstant();
        }

        /// <summary>
        /// Converts a LocalDateTime to UTC Instant using a specified DateTimeZone.
        /// </summary>
        /// <param name="localDateTime">The LocalDateTime object to convert.</param>
        /// <param name="dateTimeZone">The DateTimeZone object representing the time zone.</param>
        /// <returns>An Instant object representing the UTC time.</returns>
        /// <exception cref="ArgumentException">Thrown if the local date time is the default value.</exception>
        /// <exception cref="ArgumentNullException">Thrown if the dateTimeZone is null.</exception>
        public Instant ConvertToUtc(LocalDateTime localDateTime, DateTimeZone dateTimeZone)
        {
            if (localDateTime == default)
            {
                throw new ArgumentException("The local date time cannot be the default value.", nameof(localDateTime));
            }

            if (dateTimeZone == null)
            {
                throw new ArgumentNullException(nameof(dateTimeZone), "DateTimeZone cannot be null.");
            }

            var zonedDateTime = localDateTime.InZoneLeniently(dateTimeZone);
            return zonedDateTime.ToInstant();
        }

        /// <summary>
        /// Parses a location string in "latitude,longitude" format into a LocationDto.
        /// </summary>
        /// <param name="clientLocation">The location string to parse.</param>
        /// <returns>A LocationDto containing the parsed latitude and longitude.</returns>
        /// <exception cref="ArgumentException">Thrown if the location format is invalid or if latitude/longitude values are invalid.</exception>
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