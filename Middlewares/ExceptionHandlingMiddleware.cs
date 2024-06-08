using InterviewPuzzle.Data_Access.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;
using Serilog;

namespace InterviewPuzzle.Middlewares
{
    public class ExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException is SqlException sqlException)
                {
                    _logger.LogError(sqlException, "Database update exception: {Message}", sqlException.Message);
                    await HandleExceptionAsync(context, HttpStatusCode.Conflict, sqlException.Message);
                }
                else
                {
                    _logger.LogError(dbUpdateException, "Database update exception: {Message}", dbUpdateException.Message);
                    await HandleExceptionAsync(context, HttpStatusCode.BadRequest, dbUpdateException.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);
                await HandleExceptionAsync(context, HttpStatusCode.InternalServerError, "An unexpected error occurred.");
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, HttpStatusCode statusCode, string message)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var response = new APIResponse<string>
            {
                IsSuccess = false,
                StatusCode = statusCode,
                ErrorMessages = new List<string> { message }
            };

            var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(jsonResponse);
        }
    }
}
