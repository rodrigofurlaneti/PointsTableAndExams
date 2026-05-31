using FluentValidation;
using PointsTableAndExams.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace PointsTableAndExams.Api.Middlewares;

public sealed class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (DomainException ex)
        {
            logger.LogWarning(ex, "Domain exception occurred");
            await WriteErrorAsync(context, HttpStatusCode.BadRequest, "DOMAIN_ERROR", ex.Message);
        }
        catch (ValidationException ex)
        {
            logger.LogWarning(ex, "Validation exception occurred");
            var errors = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
            await WriteErrorAsync(context, HttpStatusCode.UnprocessableEntity, "VALIDATION_ERROR",
                "One or more validation errors occurred.", errors);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception");
            await WriteErrorAsync(context, HttpStatusCode.InternalServerError, "INTERNAL_ERROR", "An unexpected error occurred.");
        }
    }

    private static async Task WriteErrorAsync(HttpContext context, HttpStatusCode status, string code, string message, object? details = null)
    {
        context.Response.StatusCode = (int)status;
        context.Response.ContentType = "application/json";
        var body = JsonSerializer.Serialize(new { code, message, details });
        await context.Response.WriteAsync(body);
    }
}
