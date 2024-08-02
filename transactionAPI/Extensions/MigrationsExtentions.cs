using Microsoft.EntityFrameworkCore;
using transactionAPI.DataAccess.Data;

namespace transactionAPI.Extensions
{
    public static class MigrationsExtentions
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();
            using ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.Migrate();
        }
    }
}
