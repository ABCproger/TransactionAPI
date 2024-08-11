using Dapper;
using NodaTime;
using System.Data;

namespace transactionAPI.DataAccess.DateTimeHandlers
{
    /// <summary>
    /// A type handler for converting between <see cref="LocalDateTime"/> from NodaTime and <see cref="DateTime"/> for database operations.
    /// </summary>
    public class LocalDateTimeHandler : SqlMapper.ITypeHandler
    {
        /// <summary>
        /// Sets the value of the parameter to a LocalDateTime instance.
        /// Converts the LocalDateTime to DateTime and sets it as the parameter value.
        /// </summary>
        /// <param name="parameter">The database parameter to be set.</param>
        /// <param name="value">The <see cref="LocalDateTime"/> value to set</param>
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

        /// <summary>
        /// Parses the value from the database to LocalDateTime.
        /// Converts DateTime to LocalDateTime.
        /// </summary>
        /// <param name="destinationType">The type to parse to</param>
        /// <param name="value">The value to be parsed, expected to be of type DateTime.</param>
        /// <returns>The parsed value as LocalDateTime.</returns>
        /// <exception cref="InvalidCastException">Thrown when the value cannot be cast to LocalDateTime.</exception>
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
