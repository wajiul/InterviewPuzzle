using InterviewPuzzle.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

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

            catch (DomainNotFoundException ex)
            {
                await ResponseToExceptionAsync(context, ex.Message, StatusCodes.Status404NotFound);
            }
            catch (DomainAlreadyExistException ex)
            {
                await ResponseToExceptionAsync(context, ex.Message, StatusCodes.Status400BadRequest);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException is SqlException sqlException)
                {
                    await ResponseToExceptionAsync(context, sqlException.Message, StatusCodes.Status409Conflict);
                }
                else
                {
                    await ResponseToExceptionAsync(context, dbUpdateException.Message, StatusCodes.Status400BadRequest);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occured.");
                await ResponseToExceptionAsync(context, "An error occured", StatusCodes.Status500InternalServerError);
            }
        }


        private static async Task ResponseToExceptionAsync(HttpContext context, string message, int statusCode)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            await context.Response.WriteAsync(message);
        }

    }
}
