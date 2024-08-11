using Dapper;
using Microsoft.IdentityModel.Tokens;
using NodaTime;
using System.Data;

namespace transactionAPI.DataAccess.DateTimeHandlers
{
    /// <summary>
    /// A type handler for converting between <see cref="Instant"/> from NodaTime and <see cref="DateTime"/> for database operations.
    /// </summary>
    public class InstantHandler : SqlMapper.ITypeHandler
    {
        /// <summary>
        /// Sets the value of a database parameter from an <see cref="Instant"/>.
        /// </summary>
        /// <param name="parameter">The database parameter to be set.</param>
        /// <param name="value">The <see cref="Instant"/> value to set.</param>
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

        /// <summary>
        /// Parses a <see cref="DateTime"/> value to an <see cref="Instant"/>.
        /// </summary>
        /// <param name="destinationType">The type to parse to.</param>
        /// <param name="value">The <see cref="DateTime"/> value to convert.</param>
        /// <returns>The converted <see cref="Instant"/> value.</returns>
        /// <exception cref="InvalidCastException">Thrown when <paramref name="value"/> is not of type <see cref="DateTime"/>.</exception>
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
