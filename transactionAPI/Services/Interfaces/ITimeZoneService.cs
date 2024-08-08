namespace transactionAPI.Services.Interfaces
{
    using NodaTime;
    using System;

    public interface ITimeZoneService
    {
        ZonedDateTime ConvertToZonedDateTime(DateTime dateTime, double latitude, double longitude);
    }

}
