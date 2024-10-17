using System.Net;
using Newtonsoft.Json;

namespace ToDoAppAPI.Middlewares
    {
    // Middleware to handle exceptions globally in the application.
    // This class ensures that any unhandled exceptions are caught and a user-friendly message is returned.
    public class ExceptionHandlingMiddleware
        {
        private readonly RequestDelegate _next;  // Delegate representing the next middleware in the pipeline
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;  // Logger to log error details

        // Constructor that accepts the next middleware and logger as dependencies.
        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
            {
            _next = next;
            _logger = logger;
            }

        // Method to invoke the middleware. It wraps the request pipeline in a try-catch to catch unhandled exceptions.
        public async Task InvokeAsync(HttpContext context)
            {
            try
                {
                await _next(context);  // Call the next middleware in the pipeline
                }
            catch (Exception ex)
                {
                await HandleExceptionAsync(context, ex);  // If an exception occurs, handle it using HandleExceptionAsync
                }
            }

        // Method to handle exceptions and return a standardized response.
        private Task HandleExceptionAsync(HttpContext context, Exception ex)
            {
            // Log the error for debugging purposes. This could be extended to log more details or send alerts.
            _logger.LogError(ex, "An unexpected error occurred.");

            // Prepare the JSON response to send back to the client with a user-friendly error message.
            var result = JsonConvert.SerializeObject(new
                {
                error = "An unexpected error occurred. Please try again later."
                });

            // Set the response details: content type as JSON and status code to 500 (Internal Server Error).
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            // Write the response back to the client.
            return context.Response.WriteAsync(result);
            }
        }
    }
