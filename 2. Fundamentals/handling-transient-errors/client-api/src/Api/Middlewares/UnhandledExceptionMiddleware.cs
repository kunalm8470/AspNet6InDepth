using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Api.Middlewares;

public class UnhandledExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public UnhandledExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IWebHostEnvironment hostingEnvironment)
    {
        try
        {
            await _next.Invoke(context);
        }
        catch (Exception ex)
        {
            string exceptionType = ex.GetType().ToString();

            ProblemDetails details;

            /*
             *  Inspect ASPNETCORE_ENVIRONMENT environment variable
             *  looking at its value it will decide which environment it is
            */
            if (hostingEnvironment.IsDevelopment())
            {
                details = new ProblemDetails
                {
                    Type = exceptionType,
                    Detail = ex.Message,
                    Status = StatusCodes.Status500InternalServerError
                };
            }
            else
            {
                details = new ProblemDetails
                {
                    Type = exceptionType,
                    Detail = "An error has occured we are looking into it.",
                    Status = StatusCodes.Status500InternalServerError
                };
            }

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            await context.Response.WriteAsync(JsonConvert.SerializeObject(details, Formatting.Indented));
        }
    }
}
