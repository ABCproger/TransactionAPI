using Microsoft.EntityFrameworkCore;
using transactionAPI.DataAccess.Data;

namespace transactionAPI.Extensions
{
    /// <summary>
    /// Extension methods for applying database migrations in an application.
    /// </summary>
    public static class MigrationsExtentions
    {
        /// <summary>
        /// Applies pending migrations to the database.
        /// This method should be called during application startup to ensure that the database schema is up-to-date.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> instance for configuring the application pipeline.</param>
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            // Create a scope to obtain services from the dependency injection container.
            using IServiceScope scope = app.ApplicationServices.CreateScope();

            using ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            dbContext.Database.Migrate();
        }
    }
}
