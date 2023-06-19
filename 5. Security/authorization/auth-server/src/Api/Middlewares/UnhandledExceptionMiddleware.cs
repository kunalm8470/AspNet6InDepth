using Api.Constants;
using Api.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Api.Middlewares;

public class UnhandledExceptionMiddleware
{
    private readonly ILogger<UnhandledExceptionMiddleware> _logger;

    private readonly IWebHostEnvironment _hostingEnvironment;

    private readonly RequestDelegate _next;

    public UnhandledExceptionMiddleware(
        ILogger<UnhandledExceptionMiddleware> logger,
        IWebHostEnvironment hostingEnvironment,
        RequestDelegate next
    )
    {
        _logger = logger;
        _hostingEnvironment = hostingEnvironment;
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await _next.Invoke(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(ex, httpContext);
        }
    }

    #region <<< Private helper methods >>>

    private async Task HandleExceptionAsync(Exception ex, HttpContext httpContext)
    {
        bool isSuccessful = httpContext.Request.Headers.TryGetValue(
            key: CorrelationIdConstants.CORRELATIONID_HEADER,
            value: out StringValues correlationIdHeaders
        );

        string correlationIdHeader = isSuccessful ? correlationIdHeaders.ElementAt(0) : Guid.NewGuid().ToString();

        string title = "An error occurred while processing your request.";
        string errorMessageDetail = ex.Message;
        int statusCode = StatusCodes.Status500InternalServerError;
        string type = "https://tools.ietf.org/html/rfc7231#section-6.6.1";
        string instance = httpContext.Request.Path;

        switch (ex)
        {
            case UserNotFoundException:
                title = "Resource not found";
                statusCode = StatusCodes.Status404NotFound;
                type = "https://www.rfc-editor.org/rfc/rfc7231#section-6.5.4";
                break;

            case PasswordNotMatchingException:
                title = "Password not matching";
                statusCode = StatusCodes.Status400BadRequest;
                type = "https://www.rfc-editor.org/rfc/rfc7231#section-6.5.1";
                break;

            case RefreshTokenRevokedException:
                title = "Refresh token revoked";
                statusCode = StatusCodes.Status401Unauthorized;
                type = "https://www.rfc-editor.org/rfc/rfc7235#section-3.1";
                break;

            case RefreshTokenNotFoundException:
                title = "Refresh token not found";
                statusCode = StatusCodes.Status401Unauthorized;
                type = "https://www.rfc-editor.org/rfc/rfc7235#section-3.1";
                break;

            case RefreshTokenExpiredException:
                title = "Refresh token expired";
                statusCode = StatusCodes.Status401Unauthorized;
                type = "https://www.rfc-editor.org/rfc/rfc7235#section-3.1";
                break;

            case DuplicateUserException:
                title = "Conflict";
                statusCode = StatusCodes.Status409Conflict;
                type = "https://www.rfc-editor.org/rfc/rfc7231#section-6.5.8";
                break;

            default:
                if (!_hostingEnvironment.IsDevelopment())
                {
                    errorMessageDetail = "An error has occured, please try again later";
                }
                break;
        }

        if (httpContext.Request.QueryString.HasValue)
        {
            instance += httpContext.Request.QueryString.Value;
        }

        _logger.LogError("{correlationId}: Unhandled exception happened at: {exceptionTime}" +
        " {newLine}" +
        "at API: {apiName}" +
        " {newLine}" +
        " Details: {unhandledException}", correlationIdHeader, DateTime.UtcNow, Environment.NewLine, instance, Environment.NewLine, ex);

        httpContext.Response.Clear();

        ProblemDetails details = new()
        {
            Title = title,
            Type = type,
            Detail = errorMessageDetail,
            Status = statusCode,
            Instance = instance
        };

        details.Extensions.Add("traceId", Activity.Current?.Id ?? httpContext.TraceIdentifier);

        httpContext.Response.StatusCode = statusCode;

        httpContext.Response.Headers.Clear();

        httpContext.Response.Headers.Add("Content-Type", "application/problem+json");

        await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(details, Formatting.Indented));
    }

    #endregion
}
