using Dapper;
using Microsoft.IdentityModel.Tokens;
using NodaTime;
using System.Data;

namespace transactionAPI.DataAccess.DateTimeHandlers
{
    public class InstantHandler : SqlMapper.ITypeHandler
    {
        public void SetValue(IDbDataParameter parameter, object value)
        {
            if (value is Instant instant)
            {
                var dateTimeUtc = instant.ToDateTimeUtc();
                parameter.Value = dateTimeUtc;
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
                return Instant.FromDateTimeUtc(dateTime);
            }

            throw new InvalidCastException($"Cannot cast {value} to Instant.");
        }
    }
}
