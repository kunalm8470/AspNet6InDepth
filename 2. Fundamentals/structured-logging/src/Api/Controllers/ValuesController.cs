using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ValuesController : ControllerBase
{
    private readonly ILogger<ValuesController> _logger;

    public ValuesController(ILogger<ValuesController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public ActionResult<int> GetInteger()
    {
        /*
         * When you run the application from Visual studio, by default the following env variable is set to
            ASPNETCORE_ENVIRONMENT=development

        */

        _logger.LogTrace(CommonEventIDs.GetInteger, "This is a trace log {log0}, {log1}", 2, 3);

        _logger.LogDebug(CommonEventIDs.GetInteger,"This is a debug log {log0}, {log1}", 2, 3);

        _logger.LogInformation(CommonEventIDs.GetInteger, "This is an information log {log0}, {log1}", 2, 3);

        _logger.LogWarning(CommonEventIDs.GetInteger, "This is a warning log {log0}, {log1}", 2, 3);

        _logger.LogError(CommonEventIDs.GetInteger, "This is an error log {log0}, {log1}", 2, 3);

        _logger.LogCritical(CommonEventIDs.GetInteger, "This is a critical log {log0}, {log1}", 2, 3);

        return Ok(4);
    }

    [HttpGet("{id:int}")]
    public ActionResult<int> ThrowException([FromRoute] int id)
    {
        try
        {
            throw new Exception(id.ToString());
        }
        catch (Exception ex)
        {
            _logger.LogError(CommonEventIDs.ThrowException, ex, "API has threw an exception {exceptionMessage}", ex.Message);

            throw;
        }
    }
}
