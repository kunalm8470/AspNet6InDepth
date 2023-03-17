using AspNetCoreRateLimit;
using Microsoft.Extensions.Options;

namespace Api.Configurations;

public class CustomRateLimitConfiguration : RateLimitConfiguration
{
    public CustomRateLimitConfiguration(
        IOptions<IpRateLimitOptions> ipOptions,
        IOptions<ClientRateLimitOptions> clientOptions
    ) : base(ipOptions, clientOptions)
    {

    }

    public override void RegisterResolvers()
    {
        IpResolvers.Clear();

        IpResolvers.Add(new IpConnectionResolveContributor());
    }
}
