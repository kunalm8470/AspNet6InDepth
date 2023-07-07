using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

public class ErrorController : Controller
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ErrorController(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    [Route("/error")]
    public ViewResult Index()
    {
        IExceptionHandlerFeature contextFeature = _httpContextAccessor.HttpContext.Features.Get<IExceptionHandlerFeature>();

        string errorTitle = "Internal Server Error";

        string errorMessage = contextFeature.Error.Message;

        int statusCode = StatusCodes.Status500InternalServerError;

        Exception unhandledException = contextFeature.Error;

        switch (unhandledException)
        {
            case HttpRequestException hre:
                if (hre.StatusCode is null)
                {
                    errorTitle = "Internal Server Error";

                    errorMessage = "Server is down";

                    statusCode = StatusCodes.Status500InternalServerError;
                }

                else if ((int)hre.StatusCode == StatusCodes.Status401Unauthorized)
                {
                    errorTitle = "Unauthorized";

                    errorMessage = "You are unauthorized to view this page.";

                    statusCode = StatusCodes.Status401Unauthorized;
                }

                else if ((int)hre.StatusCode == StatusCodes.Status403Forbidden)
                {
                    errorTitle = "Forbidden";

                    errorMessage = "You are forbidden to view this page.";

                    statusCode = StatusCodes.Status403Forbidden;
                }

                break;
        }

        ViewData["Error"] = errorTitle;

        ViewData["Description"] = errorMessage;

        _httpContextAccessor.HttpContext.Response.StatusCode = statusCode;

        return View();
    }
}
