using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;

namespace Web.Services.HttpHandler;

public class AccessTokenHttpHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AccessTokenHttpHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Read the access token from the login cookie
        string accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        return await base.SendAsync(request, cancellationToken);
    }
}
