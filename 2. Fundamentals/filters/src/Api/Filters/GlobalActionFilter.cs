using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Filters;

public class GlobalActionFilter : IActionFilter
{
    public void OnActionExecuted(ActionExecutedContext context)
    {
        Console.WriteLine("Executed global filter");
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        Console.WriteLine("Executing global filter");
    }
}
