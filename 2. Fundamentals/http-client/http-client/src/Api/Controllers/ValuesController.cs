using Api.HttpClient;
using Api.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private static readonly System.Net.Http.HttpClient jsonTypicodeClient = new(new SocketsHttpHandler
        {
            PooledConnectionIdleTimeout = TimeSpan.FromMinutes(2) // It will check for DNS IP changes every 2 minutes
        })
        {
            BaseAddress = new Uri("https://jsonplaceholder.typicode.com/")
        };

        private readonly IHttpClientFactory _httpClientFactory;

        private readonly IJsonTypicodeClient _jsonTypicodeClient;

        public ValuesController(IHttpClientFactory httpClientFactory, IJsonTypicodeClient jsonTypicodeClient)
        {
            _httpClientFactory = httpClientFactory;
            
            _jsonTypicodeClient = jsonTypicodeClient;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            /* 
             // Named clients
            System.Net.Http.HttpClient client = _httpClientFactory.CreateClient("JsonTypicode");

            HttpResponseMessage response = await client.GetAsync("/todos");
            */

            // Typed clients

            await _jsonTypicodeClient.GetTodosAsync();

            return Ok();
        }

        [HttpGet("longrunningoperation")]
        public async Task<IActionResult> CallLongRunningOperationAsync()
        {
            try
            {
                System.Net.Http.HttpClient client = new();

                // Set the base address
                client.BaseAddress = new Uri("https://localhost:7057");

                client.Timeout = TimeSpan.FromSeconds(5);

                HttpResponseMessage response = await client.GetAsync("api/Values/dolongrunningoperation?delay=10000");

                return StatusCode((int)response.StatusCode);
            }
            catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
            {
                return StatusCode(StatusCodes.Status408RequestTimeout, new
                {
                    message = ex.Message
                });
            }
        }
    }
}
