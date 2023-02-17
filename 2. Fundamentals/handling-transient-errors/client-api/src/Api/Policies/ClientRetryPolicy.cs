using Api.Interfaces;
using Polly;
using Polly.Contrib.WaitAndRetry;
using System.Net;

namespace Api.Policies;

public class ClientRetryPolicy : IClientRetryPolicy
{
    private readonly HttpStatusCode[] _invalidStatusCodes = new[]
    {
        HttpStatusCode.BadRequest,
        HttpStatusCode.Unauthorized,
        HttpStatusCode.Forbidden,
        HttpStatusCode.NotFound,
        HttpStatusCode.InternalServerError
    };

    public IAsyncPolicy<HttpResponseMessage> ImmediateRetryPolicy { get; }

    public IAsyncPolicy<HttpResponseMessage> WaitAndRetryPolicy { get; }

    public IAsyncPolicy<HttpResponseMessage> ExponentialBackoffPolicy { get; }

    public IAsyncPolicy<HttpResponseMessage> ExponentialBackoffWithJitterPolicy { get; }

    public ClientRetryPolicy(
        IConfiguration configuration,
        ILogger<ClientRetryPolicy> logger
    )
    {
        // 1. Immediate retry strategy
        int immediateRetryCount = configuration.GetValue<int>("ImmediateRetryStrategy:RetryCount");

        ImmediateRetryPolicy = Policy.HandleResult<HttpResponseMessage>(response =>
        {
            return _invalidStatusCodes.Contains(response.StatusCode);
        })
        .RetryAsync(immediateRetryCount, onRetry: (response, retryCount) =>
        {
            logger.LogInformation("Retrying count is {retryCount}", retryCount);
        });

        // 2. Wait and retry strategy
        int waitAndRetryCount = configuration.GetValue<int>("WaitAndRetryStrategy:RetryCount");
        int waitAndRetryDuration = configuration.GetValue<int>("WaitAndRetryStrategy:WaitTime");

        WaitAndRetryPolicy = Policy.HandleResult<HttpResponseMessage>(response =>
        {
            return _invalidStatusCodes.Contains(response.StatusCode);
        })
        .WaitAndRetryAsync(waitAndRetryCount, retryCount =>
        {
            TimeSpan delay = TimeSpan.FromMilliseconds(waitAndRetryDuration);

            logger.LogInformation("Waiting for {delay} milliseconds and retrying for {retryCount} times.", delay.TotalMilliseconds, retryCount);

            return delay;
        });

        // 3. Exponential backoff retry strategy
        int exponentialBackOffRetryCount = configuration.GetValue<int>("ExponentialBackoffRetryStrategy:RetryCount");

        ExponentialBackoffPolicy = Policy.HandleResult<HttpResponseMessage>(response =>
        {
            return _invalidStatusCodes.Contains(response.StatusCode);
        })
        .WaitAndRetryAsync(exponentialBackOffRetryCount, retryCount =>
        {
            /*
             * Delay is given by the following geometric progression
             * 1. 2 ^ 0 = 1 seconds
             * 2. 2 ^ 1 = 2 seconds
             * 3. 2 ^ 2 = 4 seconds 
            */
            TimeSpan delay = TimeSpan.FromMilliseconds(Math.Pow(2, retryCount));

            logger.LogInformation("Exponentially backoff for {delay} milliseconds and retrying for {retryCount} times.", delay.TotalMilliseconds, retryCount);

            return delay;
        });

        // 4. Exponential backoff with jitter retry strategy
        ExponentialBackoffWithJitterPolicy = Policy.HandleResult<HttpResponseMessage>(response =>
        {
            return _invalidStatusCodes.Contains(response.StatusCode);
        })
        .WaitAndRetryAsync(exponentialBackOffRetryCount, retryCount =>
        {
            // Generate jittered random delays
            IEnumerable<TimeSpan> delays = Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromMilliseconds(700), retryCount);

            logger.LogInformation("Exponentially backoff with Jitter, {delays} waiting and Retrying count is {retryCount}", retryCount, string.Join(",", delays.Select(x => x.TotalMilliseconds)));

            // Take the first randomly generated jittered delay from the sequence
            return delays.First();
        });
    }
}
