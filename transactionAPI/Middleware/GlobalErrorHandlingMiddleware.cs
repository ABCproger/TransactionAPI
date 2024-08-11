using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace transactionAPI.Middleware
{
    /// <summary>
    /// Middleware for handling global errors in the ASP.NET Core application.
    /// </summary>
    public class GlobalErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalErrorHandlingMiddleware> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalErrorHandlingMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next middleware in the request pipeline.</param>
        /// <param name="logger">The logger for logging error details.</param>
        public GlobalErrorHandlingMiddleware(RequestDelegate next, ILogger<GlobalErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Invokes the middleware to handle incoming requests and exceptions.
        /// </summary>
        /// <param name="context">The HTTP context for the current request.</param>
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
