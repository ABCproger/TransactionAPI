using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace transactionAPI.Middleware
{
    public class GlobalErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalErrorHandlingMiddleware> _logger;
        public GlobalErrorHandlingMiddleware(RequestDelegate next, ILogger<GlobalErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                var details = new ProblemDetails
                {
                    Detail = "Internal Server error",
                    Instance = "Error",
                    Status = 500,
                    Title = "Server Error",
                    Type = "Error",
                };
                var response = JsonSerializer.Serialize(details);
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(response);
            }
        }
    }
}
