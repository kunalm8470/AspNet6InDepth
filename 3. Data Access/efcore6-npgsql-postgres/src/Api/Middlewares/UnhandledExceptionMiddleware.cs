using Api.Common.Constants;
using Api.Common.Exceptions;
using EntityFramework.Exceptions.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Npgsql;
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

    public async Task InvokeAsync(HttpContext httpContext)
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
        string errorMessageDetail = _hostingEnvironment.IsDevelopment() ? ex.Message : "An error has occured, please try again later";
        int statusCode = StatusCodes.Status500InternalServerError;
        string type = "https://tools.ietf.org/html/rfc7231#section-6.6.1";
        string instance = httpContext.Request.Path;

        switch (ex)
        {
            case NotFoundException ne:
                title = "Not Found";
                errorMessageDetail = ne.Message;
                statusCode = StatusCodes.Status404NotFound;
                type = "https://www.rfc-editor.org/rfc/rfc7231#section-6.5.4";
                break;

            case MaxLengthExceededException mle:
                if ((mle.InnerException as PostgresException)?.SqlState == "22001")
                {
                    title = "Max length exceeded";
                    errorMessageDetail = mle.Message;
                    statusCode = StatusCodes.Status422UnprocessableEntity;
                    type = "https://www.rfc-editor.org/rfc/rfc4918#section-11.2";
                }

                break;

            case NumericOverflowException noe:
                if ((noe.InnerException as PostgresException)?.SqlState == "22003")
                {
                    title = "Numeric overflow";
                    errorMessageDetail = noe.Message;
                    statusCode = StatusCodes.Status422UnprocessableEntity;
                    type = "https://www.rfc-editor.org/rfc/rfc4918#section-11.2";
                }
                
                break;

            case CannotInsertNullException cie:
                if ((cie.InnerException as PostgresException)?.SqlState == "23502")
                {
                    title = "Null insertion";
                    errorMessageDetail = cie.Message;
                    statusCode = StatusCodes.Status422UnprocessableEntity;
                    type = "https://www.rfc-editor.org/rfc/rfc4918#section-11.2";
                }
                
                break;

            case ReferenceConstraintException rce:
                if ((rce.InnerException as PostgresException)?.SqlState == "23503")
                {
                    title = "Referential integrity violated";
                    errorMessageDetail = rce.Message;
                    statusCode = StatusCodes.Status422UnprocessableEntity;
                    type = "https://www.rfc-editor.org/rfc/rfc4918#section-11.2";
                }
                
                break;

            case UniqueConstraintException uce:
                if ((uce.InnerException as PostgresException)?.SqlState == "23505")
                {
                    title = "Uniqueness violated";
                    errorMessageDetail = uce.Message;
                    statusCode = StatusCodes.Status409Conflict;
                    type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.8";
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
