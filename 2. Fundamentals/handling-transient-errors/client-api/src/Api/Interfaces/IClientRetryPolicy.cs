using Polly;

namespace Api.Interfaces
{
    public interface IClientRetryPolicy
    {
        public IAsyncPolicy<HttpResponseMessage> ImmediateRetryPolicy { get; }

        public IAsyncPolicy<HttpResponseMessage> WaitAndRetryPolicy { get; }

        public IAsyncPolicy<HttpResponseMessage> ExponentialBackoffPolicy { get; }

        public IAsyncPolicy<HttpResponseMessage> ExponentialBackoffWithJitterPolicy { get; }
    }
}
