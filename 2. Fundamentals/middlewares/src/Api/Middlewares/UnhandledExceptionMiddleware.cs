using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Api.Middlewares
{
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
                await _next(context);
            }
            catch(Exception ex)
            {
                ProblemDetails details;

                /*
                 *  Inspect ASPNETCORE_ENVIRONMENT environment variable
                 *  looking at its value it will decide which environment it is
                */
                if (hostingEnvironment.IsDevelopment())
                {
                    details = new ProblemDetails
                    {
                        Type = ex.GetType().ToString(),
                        Detail = ex.Message,
                        Status = 500
                    };
                }
                else
                {
                    details = new ProblemDetails
                    {
                        Type = ex.GetType().ToString(),
                        Detail = "An error has occured",
                        Status = 500
                    };
                }

                context.Response.StatusCode = 500;

                await context.Response.WriteAsync(JsonSerializer.Serialize(details));
            }
        }
    }
}
