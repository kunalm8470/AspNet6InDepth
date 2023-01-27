using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        private readonly Auth0Configuration _optionsMonitor;
        
        private readonly Auth0Configuration _optionsSnapshot;
        
        private readonly Auth0Configuration _options;

        public ValuesController(
            IConfiguration configuration,
            IOptions<Auth0Configuration> options,
            IOptionsSnapshot<Auth0Configuration> optionsSnapshot,
            IOptionsMonitor<Auth0Configuration> optionsMonitor
        )
        {
            _configuration = configuration;

            // Access the value property to read the actual configuration
            _optionsSnapshot = optionsSnapshot.Value;

            // Access the CurrentValue property to read the actual configuration
            _optionsMonitor = optionsMonitor.CurrentValue;

            // Access the value property to read the actual configuration
            _options = options.Value;
        }

        [HttpGet]
        public IActionResult GetConfigurations()
        {
            // Weakly typed
            string value = _configuration["auth0:retryCount"];

            // Utility method called GetValue, provide type explicitly
            int stronglyTypedConfiguration = _configuration.GetValue<int>("auth0:retryCount");

            string onlyInRootProp = _configuration["onlyInRoot"];

            var response = new
            {
                weakly_typed_configuration = value,
                strongly_typed_configuration = stronglyTypedConfiguration,
                options_configuration = _options.RetryCount,
                options_snapshot_configuration = _optionsSnapshot.RetryCount,
                options_monitor_configuration = _optionsMonitor.RetryCount
            };

            return Ok(response);
        }
    }
}
