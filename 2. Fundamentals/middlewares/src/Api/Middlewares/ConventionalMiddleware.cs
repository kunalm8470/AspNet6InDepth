using Api.Interfaces;

namespace Api.Middlewares
{
    public class ConventionalMiddleware
    {
        /*
            Dependency inject RequestDelegate in the constructor
        */
        private readonly RequestDelegate _next;

        public ConventionalMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /*
            Implement the invoke method
        */
        public async Task Invoke(HttpContext context, IDummyService dummyService)
        
        {
            await context.Response.WriteAsync($"From conventional middleware - {dummyService.ReturnInt()}");
        }
    }
}
