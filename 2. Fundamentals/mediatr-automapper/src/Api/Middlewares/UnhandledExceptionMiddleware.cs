using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Xml;

namespace Api.Middlewares;

public class UnhandledExceptionMiddleware
{
    private readonly RequestDelegate _next;

    private readonly ILogger<UnhandledExceptionMiddleware> _logger;
    
    private readonly IWebHostEnvironment _webHostEnvironment;

    public UnhandledExceptionMiddleware(
        RequestDelegate next,
        ILogger<UnhandledExceptionMiddleware> logger,
        IWebHostEnvironment webHostEnvironment
    )
	{
        _next = next;
        
        _logger = logger;
        
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next.Invoke(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(ex, context, _logger, _webHostEnvironment.IsDevelopment());
        }
    }

    #region <<< Private helper methods >>>

    public async Task HandleExceptionAsync(
        Exception ex,
        HttpContext context,
        ILogger<UnhandledExceptionMiddleware> logger,
        bool isDevelopmentEnvironment
    )
    {
        Task handleExceptionTask = ex switch
        {
            PersonNotFoundException pnfe => HandlePersonNotFoundException(pnfe, context, logger, isDevelopmentEnvironment),

            PersonIdMismatchException pmie => HandlePersonIdMismatchException(pmie, context, logger, isDevelopmentEnvironment),

            PersonAlreadyExistsException pae => HandlePersonAlreadyExistsException(pae, context, logger, isDevelopmentEnvironment),

            _ => HandleDefaultException(ex, context, logger, isDevelopmentEnvironment)
        };

        await handleExceptionTask;
    }

    private async Task HandlePersonAlreadyExistsException(
        PersonAlreadyExistsException exception,
        HttpContext context,
        ILogger<UnhandledExceptionMiddleware> logger,
        bool isDevelopmentEnvironment
    )
    {
        logger.LogError(exception, "Person already exists exception, Error: {errorMessage}", exception.ToString());

        await HandleErrorGeneric(
            httpContext: context,
            exceptionType: exception.GetType().ToString(),
            isDevelopmentEnvironment,
            actualErrorMessage: exception.Message,
            customErrorMessage: "Person already exists with same message id",
            statusCode: StatusCodes.Status409Conflict
        );
    }

    private static async Task HandlePersonIdMismatchException(
        PersonIdMismatchException exception,
        HttpContext context,
        ILogger<UnhandledExceptionMiddleware> logger,
        bool isDevelopmentEnvironment
    )
    {
        logger.LogError(exception, "PersonId mismatch exception, Error: {errorMessage}", exception.ToString());

        await HandleErrorGeneric(
            httpContext: context,
            exceptionType: exception.GetType().ToString(),
            isDevelopmentEnvironment,
            actualErrorMessage: exception.Message,
            customErrorMessage: "PersonId not matching with the payload",
            statusCode: StatusCodes.Status400BadRequest
        );
    }

    private static async Task HandlePersonNotFoundException(
        PersonNotFoundException exception,
        HttpContext context,
        ILogger<UnhandledExceptionMiddleware> logger,
        bool isDevelopmentEnvironment
    )
    {
        logger.LogError(exception, "Person not found exception, Error: {errorMessage}", exception.ToString());

        await HandleErrorGeneric(
            httpContext: context,
            exceptionType: exception.GetType().ToString(),
            isDevelopmentEnvironment,
            actualErrorMessage: exception.Message,
            customErrorMessage: "Person not found",
            statusCode: StatusCodes.Status404NotFound
        );
    }

    private static async Task HandleDefaultException<T>(Exception exception, HttpContext context, ILogger<T> logger, bool isDevelopmentEnvironment)
    {
        logger.LogError(exception, "Unhandled exception, Error: {errorMessage}", exception.ToString());

        await HandleErrorGeneric(
            httpContext: context,
            exceptionType: exception.GetType().ToString(),
            isDevelopmentEnvironment,
            actualErrorMessage: exception.Message,
            customErrorMessage: "An error has occured we are looking into it.",
            statusCode: StatusCodes.Status500InternalServerError
        );
    }

    private static async Task HandleErrorGeneric(
        HttpContext httpContext,
        string exceptionType,
        bool isDevelopmentEnvironment,
        string actualErrorMessage,
        string customErrorMessage,
        int statusCode
    )
    {
        ProblemDetails details;

        /*
         *  Inspect ASPNETCORE_ENVIRONMENT environment variable
         *  looking at its value it will decide which environment it is
        */
        if (isDevelopmentEnvironment)
        {
            details = new ProblemDetails
            {
                Type = exceptionType,
                Detail = actualErrorMessage,
                Status = statusCode
            };
        }
        else
        {
            details = new ProblemDetails
            {
                Type = exceptionType,
                Detail = customErrorMessage,
                Status = statusCode
            };
        }

        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(details, Newtonsoft.Json.Formatting.Indented));
    }

    #endregion
}
