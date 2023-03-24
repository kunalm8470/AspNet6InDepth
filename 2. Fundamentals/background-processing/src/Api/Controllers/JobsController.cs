using Api.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class JobsController : ControllerBase
{
    private readonly IInMemoryJobProcessorService _jobProcessorService;

    private readonly IHttpContextAccessor _httpContextAccessor;

    public JobsController(
        IInMemoryJobProcessorService jobProcessorService,
        IHttpContextAccessor httpContextAccessor
    )
    {
        _jobProcessorService = jobProcessorService;

        _httpContextAccessor = httpContextAccessor;
    }

    [HttpPost]
    public async Task<IActionResult> DoHeavyWork([FromQuery] string query)
    {
        if (string.IsNullOrEmpty(query) || string.IsNullOrWhiteSpace(query))
        {
            ModelState.AddModelError("query", "Query cannot be null or empty!");

            return BadRequest(ModelState);
        }

        CancellationToken isRequestAborted = _httpContextAccessor.HttpContext.RequestAborted;

        // Pushing the message to the channel writer, we are not waiting
        await _jobProcessorService.SendMessageAsync(query, isRequestAborted);

        // will return HTTP 202 if we do some heavy processing
        return Accepted();
    }
}
