namespace Api.DelegatingHandlers
{
    public class CustomHeaderDelegatingHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.TryAddWithoutValidation("X-Api-Source", "Demo App");

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
