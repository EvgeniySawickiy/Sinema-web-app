using System.Net;
using System.Text.Json;
using NotificationService.Core.Exceptions;

namespace NotificationService.Middleware;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var code = HttpStatusCode.InternalServerError;
        var result = string.Empty;

        switch (exception)
        {
            case SendingEmailErrorException:
                code = HttpStatusCode.BadRequest;
                result = JsonSerializer.Serialize(new { error = "Failed to send email." });
                break;

            case UserNotFoundException:
                code = HttpStatusCode.NotFound;
                result = JsonSerializer.Serialize(new { error = "User not found." });
                break;

            default:
                result = JsonSerializer.Serialize(new { error = "An unexpected error occurred." });
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;
        return context.Response.WriteAsync(result);
    }
}