using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Filters;

public class EnsureNonEmptyGuidActionFilter : Attribute, IActionFilter
{
    public void OnActionExecuted(ActionExecutedContext context)
    {
        Console.WriteLine("EnsureNonEmptyGuidActionFilter has executed the {0}", context.ActionDescriptor.DisplayName);
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        Console.WriteLine("EnsureNonEmptyGuidActionFilter is executing the {0}", context.ActionDescriptor.DisplayName);

        if (
            context.ActionArguments.ContainsKey("id")
            && context.ActionArguments.ContainsKey("id2")
            && (Guid)context.ActionArguments["id"] == Guid.Empty
            && (Guid)context.ActionArguments["id2"] == Guid.Empty
        )
        {
            context.ModelState.AddModelError("id", "Id should not be an empty guid");

            context.ModelState.AddModelError("id2", "Id2 should not be an empty guid");

            context.Result = new BadRequestObjectResult(context.ModelState);

            return;
        }
    }
}
