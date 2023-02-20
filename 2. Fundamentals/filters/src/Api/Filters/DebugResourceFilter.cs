using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Filters;

public class DebugResourceFilter : Attribute, IResourceFilter, IOrderedFilter
{
    public int Order { get; }

    public DebugResourceFilter(int order)
    {
        Order = order;
    }

    /// <summary>
    /// Called while going out (returning response)
    /// </summary>
    /// <param name="context"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void OnResourceExecuted(ResourceExecutedContext context)
    {
        Console.WriteLine("DebugResourceFilter: has finished execution, {0}", context.ActionDescriptor.DisplayName);
    }

    /// <summary>
    /// Called while going in (processing the request)
    /// </summary>
    /// <param name="context"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void OnResourceExecuting(ResourceExecutingContext context)
    {
        Console.WriteLine("DebugResourceFilter: processing execution, {0}", context.ActionDescriptor.DisplayName);

        // To short circuit from executing the other filters
        if (false)
        {
            context.ModelState.AddModelError("dummyKey", "dummyKey cannot be null");

            context.Result = new BadRequestObjectResult(context.ModelState);

            return;
        }
    }
}
