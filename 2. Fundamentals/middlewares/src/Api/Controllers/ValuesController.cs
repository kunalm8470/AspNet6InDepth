using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ValuesController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public IActionResult Get() 
        {
            _httpContextAccessor.HttpContext.Request.Headers.TryGetValue("X-Correlation-Id", out StringValues correlationIdHeaders);

            string correlationIdHeader = correlationIdHeaders.ElementAt(0);

            throw new Exception("Test exception");
        }
    }
}
