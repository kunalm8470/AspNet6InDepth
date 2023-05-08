using Api.Common.Constants;

namespace Api.Middlewares;

public class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;

    public CorrelationIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        string correlationId = Guid.NewGuid().ToString();

        /*
            Check whether correlationid is present in request headers
            or not

            If its not present, add it in request header
        */
        if (
            !context.Request.Headers.ContainsKey(CorrelationIdConstants.CORRELATIONID_HEADER)
            && !context.Request.Headers.TryGetValue(CorrelationIdConstants.CORRELATIONID_HEADER, out _)
        )
        {
            context.Request.Headers.Add(CorrelationIdConstants.CORRELATIONID_HEADER, correlationId);
        }

        context.Response.OnStarting(() =>
        {
            context.Response.Headers.Add(CorrelationIdConstants.CORRELATIONID_HEADER, correlationId);

            return Task.CompletedTask;
        });

        await _next.Invoke(context);

        /*
         * We cannot mutate the response after returning to the client
         * 
         * Because it will throw protocol violation
        */
        // context.Response.Headers.Add(CORELATIONID_HEADER, correlationId);
    }
}