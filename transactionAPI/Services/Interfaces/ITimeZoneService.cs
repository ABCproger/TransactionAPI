namespace transactionAPI.Services.Interfaces
{
    using NodaTime;
    using System;
    using transactionAPI.Data_Transfer_Objects;

    public interface ITimeZoneService
    {
        DateTimeZone GetDateTimeZone(double latitude, double longitude);
        DateTimeZone GetDateTimeZone(string timeZoneId);
        ZonedDateTime ConvertToZonedDateTime(DateTime dateTime, double latitude, double longitude);
        Instant ConvertToUtc(LocalDateTime localDateTime, double latitude, double longitude);
        Instant ConvertToUtc(LocalDateTime localDateTime, DateTimeZone dateTimeZone);
        LocationDto ParseLocation(string clientLocation);
    }

}
