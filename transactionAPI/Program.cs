using Dapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using NodaTime;
using transactionAPI;
using transactionAPI.DataAccess.Data;
using transactionAPI.DataAccess.DateTimeHandlers;
using transactionAPI.Extensions;
using transactionAPI.Middleware;
using transactionAPI.Services;
using transactionAPI.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        options.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
    });
builder.Services.AddSingleton<ITimeZoneService, TimeZoneService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddTransient<IExportDataService, ExportDataService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"), npgsqlOptions => npgsqlOptions.UseNodaTime());
});
SqlMapper.AddTypeHandler(typeof(LocalDateTime), new LocalDateTimeHandler());
SqlMapper.AddTypeHandler(typeof(Instant), new InstantHandler());
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
