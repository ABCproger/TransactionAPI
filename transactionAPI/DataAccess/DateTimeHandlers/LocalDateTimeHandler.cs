using Dapper;
using NodaTime;
using System.Data;

namespace transactionAPI.DataAccess.DateTimeHandlers
{
    using Dapper;
    using NodaTime;
    using System;
    using System.Data;

    public class LocalDateTimeHandler : SqlMapper.ITypeHandler
    {
        public void SetValue(IDbDataParameter parameter, object value)
        {
            if (value is LocalDateTime localDateTime)
            {
                parameter.Value = localDateTime.ToDateTimeUnspecified();
                parameter.DbType = DbType.DateTime;
            }
            else
            {
                parameter.Value = DBNull.Value;
            }
        }

        public object Parse(Type destinationType, object value)
        {
            if (value is DateTime dateTime)
            {
                return LocalDateTime.FromDateTime(dateTime);
            }

            throw new InvalidCastException($"Cannot cast {value} to LocalDateTime.");
        }
    }

}
