using InterviewPuzzle.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

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

            catch (NotFoundException ex)
            {
                await ResponseToExceptionAsync(context, StatusCodes.Status404NotFound, "Not Found", ex.Message);
            }
            catch (AlreadyExistException ex)
            {
                await ResponseToExceptionAsync(context,StatusCodes.Status400BadRequest, "Bad Request", ex.Message);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException is SqlException sqlException)
                {
                    await ResponseToExceptionAsync(context, StatusCodes.Status409Conflict, "Conflict", sqlException.Message);
                }
                else
                {
                    await ResponseToExceptionAsync(context, StatusCodes.Status400BadRequest, "Bad Request", dbUpdateException.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occured.");
                await ResponseToExceptionAsync(context, StatusCodes.Status500InternalServerError, "Internal Server Error", "An unexpected error occured");
            }
        }


        private static async Task ResponseToExceptionAsync(HttpContext context, int statusCode, string title, string message)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            var response = new
            {
                title = title,
                status = statusCode,
                message = message
            };

            var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            });

            await context.Response.WriteAsync(jsonResponse);
        }

    }
}
