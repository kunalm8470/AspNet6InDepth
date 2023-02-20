using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Filters;

public class EnsureNonEmptyGuidAsyncActionFilter : Attribute, IAsyncActionFilter
{
    private readonly ILogger<EnsureNonEmptyGuidAsyncActionFilter> _logger;

    public EnsureNonEmptyGuidAsyncActionFilter(ILogger<EnsureNonEmptyGuidAsyncActionFilter> logger)
    {
        _logger = logger;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        _logger.LogInformation("EnsureNonEmptyGuidAsyncActionFilter is executing the {0}", context.ActionDescriptor.DisplayName);

        await Task.Delay(2000);

        _logger.LogInformation("EnsureNonEmptyGuidAsyncActionFilter has executed the {0}", context.ActionDescriptor.DisplayName);
    }
}
