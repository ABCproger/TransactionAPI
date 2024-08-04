using Microsoft.EntityFrameworkCore;
using transactionAPI.DataAccess.Data;
using transactionAPI.Extensions;
using transactionAPI.Middleware;
using transactionAPI.Services;
using transactionAPI.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddScoped<TransactionService>();
builder.Services.AddTransient<IExportDataService, ExportDataService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection"));
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.ApplyMigrations();
app.UseHttpsRedirection();

app.UseAuthorization();
//app.UseMiddleware<GlobalErrorHandlingMiddleware>();
app.MapControllers();

app.Run();
