using Api.Interfaces;
using Api.Services;

namespace Api.Middlewares;

public class FactoryMiddleware : IMiddleware
{
    private readonly IDummyService _dummyService;

    public FactoryMiddleware(IDummyService dummyService)
    {
        _dummyService = dummyService;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        await context.Response.WriteAsync($"From factory middleware - {_dummyService.ReturnInt()}");
    }
}
