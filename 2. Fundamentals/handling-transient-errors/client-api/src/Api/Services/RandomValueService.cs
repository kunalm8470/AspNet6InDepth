using Api.Interfaces;
using Newtonsoft.Json;

namespace Api.Services;

public class RandomValueService : IRandomValueService
{
    private readonly HttpClient _client;

    private readonly IClientRetryPolicy _clientPolicy;

    public RandomValueService(HttpClient client, IClientRetryPolicy clientPolicy)
    {
        _client = client;

        _clientPolicy = clientPolicy;
    }

    public async Task<int> GetRandomIntegerAsync()
    {
        // 1. Immediate retry
        /*
            HttpResponseMessage response = await _clientPolicy.ImmediateRetryPolicy.ExecuteAsync(() =>
                _client.GetAsync("api/Values/generaterandomint")
            );
        */

        // 2. Wait and retry
        /*
            HttpResponseMessage response = await _clientPolicy.WaitAndRetryPolicy.ExecuteAsync(() =>
            _client.GetAsync("api/Values/generaterandomint")
          );
        */

        // 3. Exponential backoff retry
        /*
            HttpResponseMessage response = await _clientPolicy.ExponentialBackoffPolicy.ExecuteAsync(() =>
            _client.GetAsync("api/Values/generaterandomint")
          );
        */


        // 4. Exponential backoff with jitter retry
        /*
          HttpResponseMessage response = await _clientPolicy.ExponentialBackoffWithJitterPolicy.ExecuteAsync(() => 
            _client.GetAsync("api/Values/generaterandomint")
          );
        */

        // Wrap the _client.getAsync call inside the polly retry policy
        HttpResponseMessage response = await _clientPolicy.ExponentialBackoffWithJitterPolicy.ExecuteAsync(() =>
            _client.GetAsync("api/Values/generaterandomint")
        );

        string serializedResponse = await response.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<int>(serializedResponse);
    }
}
